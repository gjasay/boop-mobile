import { Schema, ArraySchema, type } from "@colyseus/schema";
import { Vector2 } from "../utils/Vector2";

/*-------------------
 * Schema Definitions
 --------------------*/

export class GamePieceState extends Schema
{
  @type("string") type: string; // "tadpole" or "frog"
  @type("int32") playerId: number; // 1 or 2

  constructor(type: string = null, playerId: number = null)
  {
    super();
    this.type = type;
    this.playerId = playerId;
  }
}

export class PositionState extends Schema
{
  @type("int32") x: number;
  @type("int32") y: number;

  constructor(x: number = 0, y: number = 0)
  {
    super();
    this.x = x;
    this.y = y;
  }
}

export class NeighborState extends Schema
{
  @type(PositionState) up: PositionState = null;
  @type(PositionState) down: PositionState = null;
  @type(PositionState) left: PositionState = null;
  @type(PositionState) right: PositionState = null;
  @type(PositionState) upLeft: PositionState = null;
  @type(PositionState) upRight: PositionState = null;
  @type(PositionState) downLeft: PositionState = null;
  @type(PositionState) downRight: PositionState = null;
}

export class TileState extends Schema
{
  @type(GamePieceState) gamePiece: GamePieceState = null;
  //These represent the position of the tile in the 2D array
  @type (PositionState) position = { x: 0, y: 0 };
  @type (NeighborState) neighbor = new NeighborState();
  @type ([PositionState]) neighbors = new ArraySchema<PositionState>();
}

export class HandState extends Schema
{
  @type("int32") tadpoles = 8; // Starting number of tadpoles
  @type("int32") frogs = 0; // Starting number of frogs
}

export class PlayerState extends Schema
{
  @type("int32") id: number;
  @type("string") sessionId: string;
  @type(HandState) hand = new HandState();

  constructor(id: number)
  {
    super();
    this.id = id;
  }
}

export class BoardState extends Schema
{
  @type ([TileState]) tiles = new ArraySchema<TileState>();
  @type("int32") width = 6;
  @type("int32") height = 6;
  @type("int32") tadpoles = 0;
  @type("int32") frogs = 0;
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState(1);
  @type(PlayerState) playerTwo = new PlayerState(2);
  @type("int32") currentPlayer = 1;
  @type(BoardState) board = new BoardState();
  @type("int32") winner: number = null;
}