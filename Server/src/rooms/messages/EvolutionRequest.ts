import { MyRoom } from "../MyRoom";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

export function requestEvolution(room: MyRoom, x: number, y: number, playerId: number)
{
  const state = room.state;
  const tile = GameUtils.getTile(state, new Vector2(x, y));
  const player = GameUtils.getPlayer(state, playerId);

  if (tile.gamePiece.playerId !== player.id)
  {
    console.warn("Invalid Selection: Player does not own this piece.");
    return;
  }

  if (tile.gamePiece.type === "frog")
  {
    console.warn("Invalid Selection: This piece is already a frog, silly!");
    return;
  }

  tile.gamePiece = null;
  player.hand.frogs++;
  GameUtils.switchPlayer(state, playerId);
}