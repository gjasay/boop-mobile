import { Client } from "colyseus";
import { MyRoom } from "../MyRoom";
import {
  Vector2Schema,
  GamePieceState,
  GameState,
  PlayerState,
  TileState,
} from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

export function handlePostPlacement(
  room: MyRoom,
  client: Client,
  piece: PieceMessage,
) {
  const state = room.state;
  const tile = GameUtils.getTile(state, new Vector2(piece.x, piece.y));
  const player = GameUtils.getPlayer(state, piece.playerId);
  const tilesToCheck: TileState[] = [tile];
  const extendedNeighbors = getExtendedNeighbors(state, tile);

  extendedNeighbors.forEach((neighbor) => {
    if (neighbor) tilesToCheck.push(neighbor);
  });

  console.log(`[Player ${player.id}] Checking for rows...`);
  checkForRow(room, tilesToCheck);

  if (player.hand.kittens > 0 || player.hand.cats > 0) {
    GameUtils.switchPlayer(room, piece.playerId);
    return;
  }

  if (piece.type === "kitten") {
    checkForEvolution(room, client, player);
  } else if (piece.type === "cat") {
    checkForAllPieces(room, client, player);
  }
}

function getExtendedNeighbors(
  state: GameState,
  tile: TileState,
): (TileState | null)[] {
  return [
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.up, Vector2.UP)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.down, Vector2.DOWN)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.left, Vector2.LEFT)),
    GameUtils.getTile(state, Vector2.Add(tile.neighbor.right, Vector2.RIGHT)),
    GameUtils.getTile(
      state,
      Vector2.Add(tile.neighbor.upLeft, Vector2.UP_LEFT),
    ),
    GameUtils.getTile(
      state,
      Vector2.Add(tile.neighbor.upRight, Vector2.UP_RIGHT),
    ),
    GameUtils.getTile(
      state,
      Vector2.Add(tile.neighbor.downLeft, Vector2.DOWN_LEFT),
    ),
    GameUtils.getTile(
      state,
      Vector2.Add(tile.neighbor.downRight, Vector2.DOWN_RIGHT),
    ),
  ];
}

function checkForRow(room: MyRoom, tiles: TileState[]): void {
  const state = room.state;
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

  tiles.forEach((tile) => {
    const piece = GameUtils.getPiece(state, tile);
    if (!tile || !piece) return;

    rowsToTest.forEach((row) => {
      const rowPieces: GamePieceState[] = [piece];

      row.forEach((direction) => {
        const neighborPiece: GamePieceState = GameUtils.getPiece(
          state,
          Vector2.Add(tile.coordinate, direction),
        );
        if (!neighborPiece) return;
        if (piece.playerId !== neighborPiece.playerId) return;
        rowPieces.push(neighborPiece);
      });

      if (rowPieces.length === 3) {
        const isStraightLine = rowPieces.every((piece, index, arr) => {
          if (index === 0) return true;
          const prevPiece = arr[index - 1];
          return (
            piece.coordinate.x === prevPiece.coordinate.x ||
            piece.coordinate.y === prevPiece.coordinate.y ||
            piece.coordinate.x - prevPiece.coordinate.x ===
              piece.coordinate.y - prevPiece.coordinate.y ||
            piece.coordinate.x - prevPiece.coordinate.x ===
              prevPiece.coordinate.y - piece.coordinate.y
          );
        });

        if (isStraightLine) {
          handleRowFound(room, rowPieces);
        }
      }
    });
  });
}

function handleRowFound(room: MyRoom, rowPieces: GamePieceState[]): void {
  const state = room.state;
  const player = GameUtils.getPlayer(state, rowPieces[0].playerId);
  console.log(`Player ${player.id} has a row of three!`);

  if (
    rowPieces.every((piece) => {
      return piece?.type === "cat" && !piece?.isGraduating;
    })
  ) {
    GameUtils.delcareWinner(room, player.id);
  } else {
    rowPieces.forEach((piece) => {
      console.log(
        `Removing piece at ${piece.coordinate.x}, ${piece.coordinate.y}`,
      );
      piece.isGraduating = true;
      piece.type = "cat";
      setTimeout(() => { GameUtils.removePiece(state, piece); }, 550);
    });
  }
}

function checkForAllPieces(
  room: MyRoom,
  client: Client,
  player: PlayerState,
): void {
  const state = room.state;

  if (player.hand.kittens === 0 && player.hand.cats === 0) {
    let catsOnBoard = 0;
    state.gamePieces.forEach((piece) => {
      if (piece.type === "cat" && piece.playerId === player.id) catsOnBoard++;
    });
    if (catsOnBoard >= 8) {
      GameUtils.delcareWinner(room, player.id);
    } else {
      room.sendEvolutionMessage(client);
    }
  }
}

function checkForEvolution(
  room: MyRoom,
  client: Client,
  player: PlayerState,
): void {
  if (player.hand.kittens === 0 && player.hand.cats === 0) {
    room.sendEvolutionMessage(client);
  } else {
    GameUtils.switchPlayer(room, player.id);
  }
}
