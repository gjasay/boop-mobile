import { Schema, ArraySchema, type } from "@colyseus/schema";

/*-------------------
 * Schema Definitions
 --------------------*/

export class PositionState extends Schema
{
  @type("float32") x: number;
  @type("float32") y: number;
}

export class GamePieceState extends Schema
{
  @type(PositionState) position = new PositionState();
  @type("int32") playerId: number; // 1 or 2
}

export class HandState extends Schema
{
  @type("int32") tadpoles = 8; // Starting number of tadpoles
  @type("int32") frogs = 0; // Starting number of frogs
}

export class PlayerState extends Schema
{
  @type("string") sessionId: string;
  @type(HandState) hand = new HandState();
}

export class BoardState extends Schema
{
  @type({ array: GamePieceState }) tadpoles = new ArraySchema<GamePieceState>();
  @type({ array: GamePieceState }) frogs = new ArraySchema<GamePieceState>();
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState();
  @type(PlayerState) playerTwo = new PlayerState();
  @type(BoardState) board = new BoardState();
}