import { GameState, PositionState, TileState } from "../schema/GameState";
import { Vector2 } from "../utils/Vector2";

/*-------------------------------------
* Create the gameboard for the room
---------------------------------------*/
export function createGameboard(state: GameState)
{
  const board = state.board;
  board.width = 6;
  board.height = 6;

  console.log("Creating gameboard...");
  //Create the gameboard
  for (let y = 0; y < board.height; y++) {
    for (let x = 0; x < board.width; x++) {
      let tile = new TileState();
      tile.position = new PositionState(x, y);

      state.board.tiles.push(tile);
    }
  }

  assignNeighbors(state);
}

function assignNeighbors(state: GameState): void {
  const width = state.board.width;
  const height = state.board.height;
  const tiles = state.board.tiles;

  for (let tile of tiles) {
    Vector2.directions.forEach(dir => {
      const neighborX = tile.position.x + dir.x;
      const neighborY = tile.position.y + dir.y;

      if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height) {
        switch (dir) {
          case Vector2.UP:
            tile.neighbor.up = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.DOWN:
            tile.neighbor.down = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.LEFT:
            tile.neighbor.left = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.RIGHT:
            tile.neighbor.right = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.UP_LEFT:
            tile.neighbor.upLeft = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.UP_RIGHT:
            tile.neighbor.upRight = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.DOWN_LEFT:
            tile.neighbor.downLeft = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
          case Vector2.DOWN_RIGHT:
            tile.neighbor.downRight = new PositionState(neighborX, neighborY);
            tile.neighbors.push(new PositionState(neighborX, neighborY));
            break;
        }
      }
    });
  }
}