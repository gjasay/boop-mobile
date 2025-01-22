import { Schema, ArraySchema, type } from "@colyseus/schema";

export class Vector2Schema extends Schema
{
  @type("int8") x: number;
  @type("int8") y: number;

  constructor(x: number = -1, y: number = -1)
  {
    super();
    this.x = x;
    this.y = y;
  }
}

export class GamePieceState extends Schema
{
  @type("string") type: string; // "kitten" or "cat"
  @type("int8") playerId: number; // 1 or 2
  @type(Vector2Schema) coordinate: Vector2Schema = new Vector2Schema();
  @type("boolean") isGraduating: boolean = false;
  @type("string") outOfBounds: string = null;

  constructor(coordinate: Vector2Schema = new Vector2Schema(), type: string = null, playerId: number = null)
  {
    super();
    this.coordinate = coordinate;
    this.type = type;
    this.playerId = playerId;
    this.isGraduating = false;
  }
}

export class NeighborState extends Schema
{
  @type(Vector2Schema) up: Vector2Schema = null;
  @type(Vector2Schema) down: Vector2Schema = null;
  @type(Vector2Schema) left: Vector2Schema = null;
  @type(Vector2Schema) right: Vector2Schema = null;
  @type(Vector2Schema) upLeft: Vector2Schema = null;
  @type(Vector2Schema) upRight: Vector2Schema = null;
  @type(Vector2Schema) downLeft: Vector2Schema = null;
  @type(Vector2Schema) downRight: Vector2Schema = null;
}

export class TileState extends Schema
{
  @type(Vector2Schema) coordinate = new Vector2Schema();
  @type(NeighborState) neighbor = new NeighborState();
  @type([Vector2Schema]) neighbors = new ArraySchema<Vector2Schema>();
}

export class HandState extends Schema
{
  @type("int8") kittens = 8;
  @type("int8") cats = 0;
}

export class PlayerState extends Schema
{
  @type("int8") id: number;
  @type("string") sessionId: string;
  @type(HandState) hand = new HandState();
  @type("float32") timer = 0;

  constructor(id: number)
  {
    super();
    this.id = id;
  }
}

export class GameState extends Schema
{
  @type(PlayerState) playerOne = new PlayerState(1);
  @type(PlayerState) playerTwo = new PlayerState(2);
  @type([TileState]) tiles = new ArraySchema<TileState>();
  @type([GamePieceState]) gamePieces = new ArraySchema<GamePieceState>();
  @type(Vector2Schema) boardSize = new Vector2Schema(6, 6);
  @type("int8") currentPlayer = 1;
  @type("int8") winner: number = null;
}
