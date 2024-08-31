import { Schema, ArraySchema, type } from "@colyseus/schema";

class PositionState extends Schema
{
  @type("number") x: number;
  @type("number") y: number;
}

class GamePieceState extends Schema
{
  @type(PositionState) position = new PositionState();
}

class HandState extends Schema
{
  @type({ array: GamePieceState }) tadpoles = new ArraySchema<GamePieceState>();
  @type({ array: GamePieceState }) frogs = new ArraySchema<GamePieceState>();
}

class PlayerState extends Schema
{
  @type("string") sessionId: string;
  @type(HandState) hand = new HandState();
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState();
  @type(PlayerState) playerTwo = new PlayerState();
}