import { Schema, ArraySchema, type } from "@colyseus/schema";

/*-------------------
 * Schema Definitions
 --------------------*/

export class ArrayCoordinate extends Schema
{
  @type("int32") x: number;
  @type("int32") y: number;

  constructor(x: number = -1, y: number = -1)
  {
    super();
    this.x = x;
    this.y = y;
  }
}

export class TransformPosition extends Schema
{
  @type("float32") x: number;
  @type("float32") y: number;

  constructor(x: number = 0, y: number = 0)
  {
    super();
    this.x = x;
    this.y = y;
  }
}

export class GamePieceState extends Schema
{
  @type("string") type: string; // "tadpole" or "frog"
  @type("int32") playerId: number; // 1 or 2
  @type(ArrayCoordinate) priorCoordinate: ArrayCoordinate = new ArrayCoordinate();
  @type(TransformPosition) position: TransformPosition = new TransformPosition();

  constructor(position: TransformPosition = new TransformPosition(), type: string = null, playerId: number = null)
  {
    super();
    this.position = position;
    this.type = type;
    this.playerId = playerId;
  }
}

export class NeighborState extends Schema
{
  @type(ArrayCoordinate) up: ArrayCoordinate = null;
  @type(ArrayCoordinate) down: ArrayCoordinate = null;
  @type(ArrayCoordinate) left: ArrayCoordinate = null;
  @type(ArrayCoordinate) right: ArrayCoordinate = null;
  @type(ArrayCoordinate) upLeft: ArrayCoordinate = null;
  @type(ArrayCoordinate) upRight: ArrayCoordinate = null;
  @type(ArrayCoordinate) downLeft: ArrayCoordinate = null;
  @type(ArrayCoordinate) downRight: ArrayCoordinate = null;
}

export class TileState extends Schema
{
  @type(GamePieceState) gamePiece: GamePieceState = new GamePieceState();
  //These represent the position of the tile in the 2D array
  @type(ArrayCoordinate) arrayPosition = new ArrayCoordinate();
  @type(TransformPosition) position = new TransformPosition();
  @type(NeighborState) neighbor = new NeighborState();
  @type([ArrayCoordinate]) neighbors = new ArraySchema<ArrayCoordinate>();
  @type("string") outOfBounds: string = null;
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
  @type("float32") timer = 0;

  constructor(id: number)
  {
    super();
    this.id = id;
  }
}

export class BoardState extends Schema
{
  @type([TileState]) tiles = new ArraySchema<TileState>();
  @type("int32") width = 6;
  @type("int32") height = 6;
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState(1);
  @type(PlayerState) playerTwo = new PlayerState(2);
  @type("int32") currentPlayer = 1;
  @type(BoardState) board = new BoardState();
  @type("int32") winner: number = null;
}