import { MyRoom } from "../MyRoom";
import {
  GameState,
  Vector2Schema,
  TileState,
  GamePieceState,
} from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

/**------------------------------------
 * Create the gameboard for the room
 ---------------------------------------*/

export function initializeRoom(room: MyRoom, options: { time: number }): void {
  const state = room.state;

  createGameboard(room);
  assignNeighbors(state);
  setupTimers(room, options.time * 60); // Convert minutes to seconds
}

function createGameboard(room: MyRoom): void {
  const state = room.state;

  const width = state.boardSize.x;
  const height = state.boardSize.y;

  console.log("Creating gameboard tiles...");
  for (let y = 0; y < height; y++) {
    for (let x = 0; x < width; x++) {
      let tile = new TileState();
      tile.coordinate = new Vector2Schema(x, y);
      state.tiles.push(tile);
    }
  }

  console.log("Creating gameboard pieces...");
  console.log(state.gamePieces.length);
  for (let i = 0; i < 16; i++) {
    if (i < 8) {
      state.gamePieces.push(new GamePieceState(new Vector2Schema(), "kitten", 1));
    } else {
      state.gamePieces.push(new GamePieceState(new Vector2Schema(), "kitten", 2));
    }
  }
  console.log(state.gamePieces.length);
  console.log("Gameboard created.");
}

function assignNeighbors(state: GameState): void {
  const width = state.boardSize.x;
  const height = state.boardSize.y;
  const tiles = state.tiles;

  for (let tile of tiles) {
    Vector2.directions.forEach((dir) => {
      const neighborX = tile.coordinate.x + dir.x;
      const neighborY = tile.coordinate.y + dir.y;

      if (
        neighborX >= 0 &&
        neighborX < width &&
        neighborY >= 0 &&
        neighborY < height
      ) {
        switch (dir) {
          case Vector2.UP:
            tile.neighbor.up = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.DOWN:
            tile.neighbor.down = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.LEFT:
            tile.neighbor.left = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.RIGHT:
            tile.neighbor.right = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.UP_LEFT:
            tile.neighbor.upLeft = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.UP_RIGHT:
            tile.neighbor.upRight = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.DOWN_LEFT:
            tile.neighbor.downLeft = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
          case Vector2.DOWN_RIGHT:
            tile.neighbor.downRight = new Vector2Schema(neighborX, neighborY);
            tile.neighbors.push(new Vector2Schema(neighborX, neighborY));
            break;
        }
      }
    });
  }
}

function setupTimers(room: MyRoom, time: number): void {
  const state = room.state;
  state.playerOne.timer = time;
  state.playerTwo.timer = time;

  GameUtils.initializePlayerTimers(room);
}
