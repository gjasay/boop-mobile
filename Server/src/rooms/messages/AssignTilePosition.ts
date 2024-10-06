import { GameState, TransformPosition } from "../schema/GameState";
import { GameUtils } from "../utils/GameUtils";
import { Vector2 } from "../utils/Vector2";

export function assignTilePosition(state: GameState, msg: TilePositionMessage): void
{
  const tile = GameUtils.getTile(state, new Vector2(msg.arrayX, msg.arrayY));
  tile.position = new TransformPosition(msg.transformX, msg.transformY);

  console.log(`Tile at (${msg.arrayX}, ${msg.arrayY}) assigned position (${msg.transformX}, ${msg.transformY})`);
}