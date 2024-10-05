import { Client } from "colyseus";
import { MyRoom } from "../MyRoom";
import { GamePieceState, GameState, PlayerState, PositionState, TileState } from "../schema/GameState";
import { Vector2 } from "../utils/Vector2";
import { GameUtils } from "../utils/GameUtils";

/*------------------------------------------------------------------
* Handle a request to place a piece on the board
* @param x: The x array position of the tile to place the piece on
* @param y: The y array position of the tile to place the piece on
* @param pieceType: The type of piece to place
* @param playerId: The ID of the player placing the piece
--------------------------------------------------------------------*/
export function handlePlacementRequest(room: MyRoom, client: Client, x: number, y: number, pieceType: string, playerId: number): void
{
  const state = room.state;
  const tile = GameUtils.getTile(state, new PositionState(x, y));
  const player = GameUtils.getPlayer(state, playerId);

  if (!isValidMove(state, tile, player)) return;

  placePiece(room, client, tile, player, pieceType);
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
function placePiece(room: MyRoom, client: Client, tile: TileState, player: PlayerState, pieceType: string): void
{
  const state = room.state;
  if (pieceType === "tadpole" && player.hand.tadpoles > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.tadpoles--;
    checkForEvolution(room, client, player);
  } else if (pieceType === "frog" && player.hand.frogs > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.frogs--;
    checkForAllFrogs(room, client, player);
  } else {
    console.warn(`Invalid move: Player does not have any ${pieceType}s left.`);
  }
}

function checkForAllFrogs(room: MyRoom, client: Client, player: PlayerState): void
{
  const state = room.state;

  if (player.hand.tadpoles === 0 && player.hand.frogs === 0) {
    let frogsOnBoard = 0;
    state.board.tiles.forEach((tile) =>
    {
      if (tile.gamePiece?.type === "frog" && tile.gamePiece.playerId === player.id) frogsOnBoard++;
    });

    if (frogsOnBoard >= 8) {
      GameUtils.delcareWinner(state, player.id);
    } else {
      room.sendEvolutionMessage(client);
    }
  }
}

function checkForEvolution(room: MyRoom, client: Client, player: PlayerState): void
{
  if (player.hand.tadpoles === 0 && player.hand.frogs === 0) {
    room.sendEvolutionMessage(client);
  } else {
    GameUtils.switchPlayer(room.state, player.id);
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
  tile.gamePiece = new GamePieceState(type, playerId); // Place the piece on the tile

  //Process game logic
  pushTileNeighbors(state, tile);

  const tilesToCheck: TileState[] = [tile];
  const extendedNeighbors = getExtendedNeighbors(state, tile);

  extendedNeighbors.forEach((neighbor) =>
  {
    if (neighbor) tilesToCheck.push(neighbor);
  });

  checkForRow(state, tilesToCheck);
}

/*---------------------------------------------------------
* Get the neighbors two tiles away from the specified tile
* @param state: The current game state
* @param tile: The tile to get the neighbors of
* @returns An array of the neighbors two tiles away
----------------------------------------------------------*/
function getExtendedNeighbors(state: GameState, tile: TileState): (TileState | null)[]
{
  return [
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.up, Vector2.UP)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.down, Vector2.DOWN)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.left, Vector2.LEFT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.right, Vector2.RIGHT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.upLeft, Vector2.UP_LEFT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.upRight, Vector2.UP_RIGHT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.downLeft, Vector2.DOWN_LEFT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.downRight, Vector2.DOWN_RIGHT))
  ];
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

    const direction = Vector2.Subtract(neighborTile.position, tile.position) as Vector2;
    const destinationPosition = Vector2.Add(neighborTile.position, direction);

    if (GameUtils.isOutOfBounds(state, destinationPosition)) {
      handleOutOfBounds(state, neighborTile);
      return;
    }

    const destinationTile = GameUtils.getTile(state, destinationPosition);
    if (!destinationTile) return;

    if (tile.gamePiece?.type === "tadpole" && neighborTile.gamePiece?.type === "frog") return;

    if (!destinationTile.gamePiece) {
      destinationTile.gamePiece = neighborTile.gamePiece;
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

/*---------------------------------------------------------
* Check for rows of three of the same type
* @param state: The current game state
* @param tiles: The tiles to check for three in a row
----------------------------------------------------------*/
function checkForRow(state: GameState, tiles: TileState[]): void
{
  const rowsToTest: Vector2[][] = [
    [Vector2.UP, Vector2.DOWN],
    [Vector2.LEFT, Vector2.RIGHT],
    [Vector2.UP_LEFT, Vector2.DOWN_RIGHT],
    [Vector2.UP_RIGHT, Vector2.DOWN_LEFT],
    [Vector2.UP, Vector2.Multiply(Vector2.UP, 2) as Vector2],
    [Vector2.DOWN, Vector2.Multiply(Vector2.DOWN, 2) as Vector2],
    [Vector2.LEFT, Vector2.Multiply(Vector2.LEFT, 2) as Vector2],
    [Vector2.RIGHT, Vector2.Multiply(Vector2.RIGHT, 2) as Vector2],
    [Vector2.UP_LEFT, Vector2.Multiply(Vector2.UP_LEFT, 2) as Vector2],
    [Vector2.UP_RIGHT, Vector2.Multiply(Vector2.UP_RIGHT, 2) as Vector2],
    [Vector2.DOWN_LEFT, Vector2.Multiply(Vector2.DOWN_LEFT, 2) as Vector2],
    [Vector2.DOWN_RIGHT, Vector2.Multiply(Vector2.DOWN_RIGHT, 2) as Vector2],
  ];

  tiles.forEach((tile) =>
  {
    if (!tile || !tile.gamePiece) return;

    rowsToTest.forEach((row) =>
    {
      const rowPieces: TileState[] = [tile];

      row.forEach((direction) =>
      {
        const neighbor = GameUtils.getTile(state, Vector2.Add(tile.position, direction));

        if (!neighbor || !neighbor.gamePiece || !tile.gamePiece) return;
        if (neighbor.gamePiece.playerId !== tile.gamePiece.playerId) return;

        rowPieces.push(neighbor);
      });

      if (rowPieces.length === 3) {
        handleRowFound(state, tile, rowPieces);
      }
    });
  });
}

/*---------------------------------------------------------
* Handle a row of three pieces being found
* If the pieces are tadpoles, they are removed from the 
* board and three frogs are added to the player's hand
* If the pieces are frogs, the player who placed the frogs win the game
* @param state: The current game state
* @param tile: The tile that was placed
* @param rowPieces: The pieces in the row
----------------------------------------------------------*/
function handleRowFound(state: GameState, tile: TileState, rowPieces: TileState[]): void
{
  console.log(`Row of ${rowPieces.length} found!`);
  const player = GameUtils.getPlayer(state, tile.gamePiece.playerId);

  if (rowPieces.every((piece) => piece.gamePiece?.type === "frog")) {
    GameUtils.delcareWinner(state, player.id);
  } else {
    rowPieces.forEach((piece) =>
    {
      console.log(`Removing piece at ${piece.position.x}, ${piece.position.y}`);

      player.hand.frogs++;
      piece.gamePiece = null;
    });
  }
}
