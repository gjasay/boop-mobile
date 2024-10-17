import { Client } from "colyseus";
import { MyRoom } from "../MyRoom";
import { GameState, PlayerState, TileState } from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

export function handlePostPlacement(room: MyRoom, client: Client, piece: PieceMessage)
{
  const state = room.state;
  const tile = GameUtils.getTile(state, new Vector2(piece.x, piece.y));
  const player = GameUtils.getPlayer(state, piece.playerId);

  const tilesToCheck: TileState[] = [tile];
  const extendedNeighbors = getExtendedNeighbors(state, tile);

  extendedNeighbors.forEach((neighbor) =>
  {
    if (neighbor) tilesToCheck.push(neighbor);
  });

  checkForRow(state, tilesToCheck);

  if (player.hand.tadpoles > 0 || player.hand.frogs > 0) {
    GameUtils.switchPlayer(state, piece.playerId);
    return;
  }

  if (piece.type === "tadpole") {
    checkForEvolution(room, client, player);
  } else if (piece.type === "frog") {
    checkForAllFrogs(room, client, player);
  }
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

    console.log(`Checking for rows of three for tile ${tile.arrayPosition.x}, ${tile.arrayPosition.y}`);

    rowsToTest.forEach((row) =>
    {
      const rowPieces: TileState[] = [tile];

      row.forEach((direction) =>
      {
        console.log(`Checking direction ${direction.x}, ${direction.y} for tile ${tile.arrayPosition.x}, ${tile.arrayPosition.y}`);
        const neighbor = GameUtils.getTile(state, Vector2.Add(tile.arrayPosition, direction));

        if (!neighbor) {
          console.log(`Neighbor ${tile.arrayPosition.x + direction.x}, ${tile.arrayPosition.y + direction.y} does not exist`);
          return;
        };

        if (!neighbor.gamePiece) {
          console.log(`Neighbor ${neighbor.arrayPosition.x}, ${neighbor.arrayPosition.y} does not have a game piece`);
          return;
        }

        if (!tile.gamePiece) {
          console.log(`Tile ${tile.arrayPosition.x}, ${tile.arrayPosition.y} does not have a game piece`);
          return;
        }

        if (neighbor.gamePiece.playerId !== tile.gamePiece.playerId) {
          console.log(`Neighbor ${neighbor.arrayPosition.x}, ${neighbor.arrayPosition.y} is not the same player`);
          return;
        }

        rowPieces.push(neighbor);
      });

      if (rowPieces.length === 3) {
        // Ensure the pieces are in a straight line
        const isStraightLine = rowPieces.every((piece, index, arr) =>
        {
          if (index === 0) return true;
          const prevPiece = arr[index - 1];
          return (
            piece.arrayPosition.x === prevPiece.arrayPosition.x ||
            piece.arrayPosition.y === prevPiece.arrayPosition.y ||
            (piece.arrayPosition.x - prevPiece.arrayPosition.x === piece.arrayPosition.y - prevPiece.arrayPosition.y) ||
            (piece.arrayPosition.x - prevPiece.arrayPosition.x === prevPiece.arrayPosition.y - piece.arrayPosition.y)
          );
        });

        if (isStraightLine) {
          handleRowFound(state, tile, rowPieces);
        }
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
      console.log(`Removing piece at ${piece.arrayPosition.x}, ${piece.arrayPosition.y}`);

      player.hand.frogs++;
      piece.gamePiece = null;
    });
  }
}

/*---------------------------------------------------------
* Check if the player has 8 frogs on the board
* If they do, they win the game
* If not, they are prompted to evolve a tadpole
* @param room: The room the player is in
* @param client: The client to send the message to
* @param player: The player to check for frogs
----------------------------------------------------------*/
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

/*---------------------------------------------------------
* Check if player has all pieces on the board
* If they do, they are prompted to evolve a tadpole
* If not, the next player is switched to
* @param room: The room the player is in
* @param client: The client to send the message to
* @param player: The player to check for pieces
----------------------------------------------------------*/
function checkForEvolution(room: MyRoom, client: Client, player: PlayerState): void
{
  if (player.hand.tadpoles === 0 && player.hand.frogs === 0) {
    room.sendEvolutionMessage(client);
  } else {
    GameUtils.switchPlayer(room.state, player.id);
  }
}