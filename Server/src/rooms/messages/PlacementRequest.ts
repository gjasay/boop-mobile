import { MyRoom } from "../MyRoom";
import { GamePieceState, GameState, PlayerState, ArrayCoordinate, TileState } from "../schema/GameState";
import { Vector2 } from "../utils/Vector2";
import { GameUtils } from "../utils/GameUtils";
import { Client } from "colyseus";
import { handlePostPlacement } from "./PostPlacementLogic";

const movingNeighbors: TileState[] = [];

/*------------------------------------------------------------------
* Handle a request to place a piece on the board
* @param x: The x array position of the tile to place the piece on
* @param y: The y array position of the tile to place the piece on
* @param pieceType: The type of piece to place
* @param playerId: The ID of the player placing the piece
--------------------------------------------------------------------*/
export async function handlePlacementRequest(room: MyRoom, client: Client, piece: PieceMessage): Promise<void>
{
  const state = room.state;
  const tile = GameUtils.getTile(state, new ArrayCoordinate(piece.x, piece.y));
  const player = GameUtils.getPlayer(state, piece.playerId);

  if (!isValidMove(state, tile, player)) return;

  placePiece(room, tile, player, piece.type);

  if (movingNeighbors.length > 0) {
    console.log(`[Player ${player.id}] Please wait for pieces to move...`);
    await waitForPiecesToMove(movingNeighbors);
    handlePostPlacement(room, client, piece);
    return;
  } else {
    console.log(`[Player ${player.id}] Piece does not need to move`);
    handlePostPlacement(room, client, piece);
  }

}

async function waitForPiecesToMove(tiles: TileState[]): Promise<void>
{
  return new Promise((resolve) =>
  {
    const interval = setInterval(() =>
    {
      const allMoved = tiles.every((tile) => !tile.gamePiece?.priorCoordinate);
      if (allMoved) {
        clearInterval(interval);
        resolve();
      }
    }, 100);
  });
}

/*---------------------------------------------------------
* Check if a move is valid
* @param state: The current game state
* @param tile: The tile to place the piece on
* @param player: The player placing the piece
* @param playerId: The ID of the player placing the piece
* @returns True if the move is valid, false otherwise
----------------------------------------------------------*/
function isValidMove(state: GameState, tile: TileState | null, player: PlayerState): boolean
{
  if (!state.currentPlayer) {
    console.error("Current player is null.");
    return false;
  }

  if (!tile || tile.gamePiece) {
    console.warn("Invalid move: Tile is occupied or does not exist.");
    return false;
  }

  if (!player) {
    console.warn("Invalid move: Player does not exist.");
    return false;
  }

  if (player.id !== state.currentPlayer) {
    console.warn("Invalid move: It is not this player's turn.");
    return false;
  }

  return true;
}

/*---------------------------------------------------------
* Place a piece on the board
* @param room: The current room
* @param client: The client that placed the piece
* @param tile: The tile to place the piece on
* @param player: The player placing the piece
* @param pieceType: The type of piece to place
----------------------------------------------------------*/
function placePiece(room: MyRoom, tile: TileState, player: PlayerState, pieceType: string): void
{
  const state = room.state;
  if (pieceType === "tadpole" && player.hand.tadpoles > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.tadpoles--;
  } else if (pieceType === "frog" && player.hand.frogs > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.frogs--;
  } else {
    console.warn(`Invalid move: Player does not have any ${pieceType}s left.`);
  }
}

/*---------------------------------------------------------
* Place a piece on a tile
* @param tile: The tile to place the piece on
* @param playerId: The ID of the player placing the piece
* @param type: The type of piece to place
----------------------------------------------------------*/
function setPieceOnTile(state: GameState, tile: TileState, playerId: number, type: string): void
{
  tile.gamePiece = new GamePieceState(tile.position, type, playerId); // Place the piece on the tile

  console.log(`Game piece at transform position: (${tile.gamePiece.position.x}, ${tile.gamePiece.position.y})`);

  tile.gamePiece.priorCoordinate = null;

  //Process game logic
  pushTileNeighbors(state, tile);
}



/*---------------------------------------------------------
* Push the neighbors of a tile one tile away from the tile
* @param state: The current game state
* @param tile: The tile to push the neighbors of
----------------------------------------------------------*/
function pushTileNeighbors(state: GameState, tile: TileState): void
{
  if (!tile) return;

  tile.neighbors.forEach((neighbor) =>
  {
    if (!neighbor) return;

    const neighborTile = GameUtils.getTile(state, neighbor);
    if (!neighborTile || !neighborTile.gamePiece) return;

    const direction = Vector2.Subtract(neighborTile.arrayPosition, tile.arrayPosition) as Vector2;
    const destinationPosition = Vector2.Add(neighborTile.arrayPosition, direction);

    if (GameUtils.isOutOfBounds(state, destinationPosition)) {
      handleOutOfBounds(state, neighborTile);
      return;
    }

    const destinationTile = GameUtils.getTile(state, destinationPosition);
    if (!destinationTile) return;

    if (tile.gamePiece?.type === "tadpole" && neighborTile.gamePiece?.type === "frog") return; // Tadpoles cannot push frogs

    if (!destinationTile.gamePiece) {
      movingNeighbors.push(destinationTile);
      destinationTile.gamePiece = neighborTile.gamePiece;
      destinationTile.gamePiece.priorCoordinate = neighborTile.arrayPosition as ArrayCoordinate;
      neighborTile.gamePiece = null;
    }
  });
}

/*----------------------------------------------------------
* Handle a piece being pushed out of bounds
* @param state: The current game state
* @param tile: The tile that the piece is being pushed from
-----------------------------------------------------------*/
function handleOutOfBounds(state: GameState, tile: TileState): void
{
  if (!tile.gamePiece) return;

  switch (tile.gamePiece.type) {
    case "tadpole":
      if (tile.gamePiece.playerId === 1) {
        state.playerOne.hand.tadpoles++;
      } else if (tile.gamePiece.playerId === 2) {
        state.playerTwo.hand.tadpoles++;
      } else {
        console.error("Invalid player ID.");
      }
      break;
    case "frog":
      if (tile.gamePiece.playerId === 1) {
        state.playerOne.hand.frogs++;
      } else if (tile.gamePiece.playerId === 2) {
        state.playerTwo.hand.frogs++;
      } else {
        console.error("Invalid player ID.");
      }
      break;
  }

  tile.gamePiece = null;
}