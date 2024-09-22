import { Schema, ArraySchema, type } from "@colyseus/schema";

/*-------------------
 * Schema Definitions
 --------------------*/

export class GamePieceState extends Schema
{
  @type("string") type: string; // "tadpole" or "frog"
  @type("int32") playerId: number; // 1 or 2
}

export class TileState extends Schema
{
  @type(GamePieceState) gamePiece: GamePieceState = null;
  //These represent the position of the tile in the 2D array
  @type("int32") x: number; 
  @type("int32") y: number;
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
  @type ([TileState]) tiles = new ArraySchema<TileState>();
  @type("int32") width = 8;
  @type("int32") height = 8;
  @type("int32") tadpoles = 0;
  @type("int32") frogs = 0;
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState();
  @type(PlayerState) playerTwo = new PlayerState();
  @type("int32") currentPlayer = 1;
  @type(BoardState) board = new BoardState();
}