import { GameState, PositionState, TileState } from "../schema/GameState";
import { Vector2 } from "./Vector2";

export class GameUtils
{
  /*---------------------------------------------------------
  * Get a player from the game state
  * @param state: The current game state
  * @param playerId: The ID of the player to get
  * @returns The player with the specified ID or null
  ----------------------------------------------------------*/
  static getPlayer(state: GameState, playerId: number)
  {
    return playerId === 1 ? state.playerOne : playerId === 2 ? state.playerTwo : null;
  }

  /*---------------------------------------------------------
  * Get a tile from the board
  * @param state: The current game state
  * @param position: The position of the tile to get
  * @returns The tile at the specified position or null
  ----------------------------------------------------------*/
  static getTile(state: GameState, position: PositionState | Vector2): TileState | null
  {
    if (!position) return null;

    const tile = state.board.tiles[position.y * state.board.width + position.x];

    if (!tile) {
      console.warn("Tile does not exist.");
      return null;
    }

    return tile;
  }

  /*--------------------------------------------------------------
  * Check if an array position is outside the bounds of the board
  * @param state: The current game state
  * @param position: The position to check
  ---------------------------------------------------------------*/
  static isOutOfBounds(state: GameState, position: Vector2): boolean
  {
    return position.x < 0 || position.x >= state.board.width || position.y < 0 || position.y >= state.board.height;
  }

  /*---------------------------------------------------------
  * Switch the current player
  * @param state: The current game state
  * @param playerId: The ID of the player to switch to
  ----------------------------------------------------------*/
  static switchPlayer(state: GameState, playerId: number): void
  {
    state.currentPlayer = playerId === 1 ? 2 : playerId === 2 ? 1 : null;
  }

  static delcareWinner(state: GameState, playerId: number): void
  {
    state.winner = playerId;
    console.log("Player " + playerId + " wins!");
  }
}


