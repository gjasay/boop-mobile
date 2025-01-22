import { MyRoom } from "../MyRoom";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

export function requestEvolution(room: MyRoom, pieceMsg: EvolutionMessage): void
{
  const state = room.state;
  const piece = GameUtils.getPiece(state, new Vector2(pieceMsg.x, pieceMsg.y));
  const player = GameUtils.getPlayer(state, pieceMsg.playerId);

  if (!piece)
  {
    console.error("Invalid Selection: No piece found at coordinate");
    return;
  }

  if (!player)
  {
    console.error("Invalid Selection: Player does not exist.");
    return;
  }

  if (piece.playerId !== player.id)
  {
    console.warn("Invalid Selection: Player does not own this piece.");
    return;
  }

  if (piece.type === "cat")
  {
    console.warn("Invalid Selection: This piece is already a cat, silly!");
    return;
  }

  GameUtils.removePiece(piece);
  player.hand.cats++;
  GameUtils.switchPlayer(room, pieceMsg.playerId);
}
