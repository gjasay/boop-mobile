import { GameState, ArrayCoordinate, TileState } from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

/**------------------------------------
* Create the gameboard for the room
---------------------------------------*/

export function initializeRoom(state: GameState, settings: RoomSettings): void
{
  createGameboard(state);
  assignNeighbors(state);
  setupTimers(state, settings.time);
}

function createGameboard(state: GameState): void
{
  const board = state.board;
  board.width = 6;
  board.height = 6;

  console.log("Creating gameboard...");
  //Create the gameboard
  for (let y = 0; y < board.height; y++) {
    for (let x = 0; x < board.width; x++) {
      let tile = new TileState();
      tile.arrayPosition = new ArrayCoordinate(x, y);
      tile.gamePiece = null;

      state.board.tiles.push(tile);
    }
  }
}

function assignNeighbors(state: GameState): void {
  const width = state.board.width;
  const height = state.board.height;
  const tiles = state.board.tiles;

  for (let tile of tiles) {
    Vector2.directions.forEach(dir => {
      const neighborX = tile.arrayPosition.x + dir.x;
      const neighborY = tile.arrayPosition.y + dir.y;

      if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height) {
        switch (dir) {
          case Vector2.UP:
            tile.neighbor.up = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.DOWN:
            tile.neighbor.down = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.LEFT:
            tile.neighbor.left = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.RIGHT:
            tile.neighbor.right = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.UP_LEFT:
            tile.neighbor.upLeft = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.UP_RIGHT:
            tile.neighbor.upRight = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.DOWN_LEFT:
            tile.neighbor.downLeft = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
          case Vector2.DOWN_RIGHT:
            tile.neighbor.downRight = new ArrayCoordinate(neighborX, neighborY);
            tile.neighbors.push(new ArrayCoordinate(neighborX, neighborY));
            break;
        }
      }
    });
  }
}

function setupTimers(state: GameState, time: number): void
{
  state.playerOne.timer = time;
  state.playerTwo.timer = time;

  GameUtils.initializePlayerTimers(state);
}