import { GameState } from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";

export function handlePieceMoved(state: GameState, coordinate: Coordinate) {
    const tile = GameUtils.getTile(state, coordinate);
    if (tile && tile.gamePiece) {
      console.log(`Player ${tile.gamePiece.playerId}'s ${tile.gamePiece.type} moved to (${coordinate.x}, ${coordinate.y})`);
      tile.gamePiece.priorCoordinate = null;
      tile.outOfBounds = null;
    } else if (tile) {
      tile.outOfBounds = null;
    } else {
      console.log("Error: No tile found at coordinate: ", coordinate);
    }
}