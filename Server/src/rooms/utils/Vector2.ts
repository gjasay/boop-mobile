import { Vector2Schema } from "../schema/GameState";

export class Vector2
{
  x: number;
  y: number;

  constructor(x: number, y: number)
  {
    this.x = x;
    this.y = y;
  }

  public static ZERO: Vector2 = new Vector2(0, 0);
  public static UP: Vector2 = new Vector2(0, 1);
  public static DOWN: Vector2 = new Vector2(0, -1);
  public static LEFT: Vector2 = new Vector2(-1, 0);
  public static RIGHT: Vector2 = new Vector2(1, 0);
  public static UP_LEFT: Vector2 = new Vector2(-1, 1);
  public static UP_RIGHT: Vector2 = new Vector2(1, 1);
  public static DOWN_LEFT: Vector2 = new Vector2(-1, -1);
  public static DOWN_RIGHT: Vector2 = new Vector2(1, -1);

  public static directions: Vector2[] = [
    Vector2.UP,
    Vector2.DOWN,
    Vector2.LEFT,
    Vector2.RIGHT,
    Vector2.UP_LEFT,
    Vector2.UP_RIGHT,
    Vector2.DOWN_LEFT,
    Vector2.DOWN_RIGHT
  ];

  /*---------------------------
  * Vector2 Math Operations
  -----------------------------*/

  /*---------------------------------------------------------
  * Add two vectors together
  * @param a - The first vector or postion state
  * @param b - The second vector or position state
  * @return type of parameter a - Vector2 or PositionState
  ---------------------------------------------------------*/
  public static Add(a: Vector2 | Vector2Schema, b: Vector2 | Vector2Schema): Vector2 | Vector2Schema
  {
    if (a instanceof Vector2Schema) {
      return new Vector2Schema(a.x + b.x, a.y + b.y);
    } else if (a instanceof Vector2) {
      return new Vector2(a.x + b.x, a.y + b.y);
    }
  }

  /*---------------------------------------------------------
  * Subtract two vectors
  * @param a - The first vector or postion state
  * @param b - The second vector or position state
  * @return type of parameter a - Vector2 or PositionState
  ---------------------------------------------------------*/
  public static Subtract(a: Vector2 | Vector2Schema, b: Vector2 | Vector2Schema): Vector2 | Vector2Schema
  {
    if (a instanceof Vector2Schema) {
      return new Vector2Schema(a.x - b.x, a.y - b.y);
    } else if (a instanceof Vector2) {
      return new Vector2(a.x - b.x, a.y - b.y);
    }
  }

  /*---------------------------------------------------------
  * Multiply a vector by a scalar
  * @param a - The vector to multiply
  * @param b - The scalar to multiply by
  * @return type of parameter a - Vector2 or PositionState
  ----------------------------------------------------------*/
  public static Multiply(a: Vector2 | Vector2Schema | Vector2[] | Vector2Schema[], b: number): Vector2 | Vector2Schema | Vector2[] | Vector2Schema[]
  {
    if (Array.isArray(a)) {
      return a.map((vector) =>
      {
        if (vector instanceof Vector2Schema) {
          return new Vector2Schema(vector.x * b, vector.y * b);
        } else if (vector instanceof Vector2) {
          return new Vector2(vector.x * b, vector.y * b);
        }
      });
    } else {
      if (a instanceof Vector2Schema) {
        return new Vector2Schema(a.x * b, a.y * b);
      } else if (a instanceof Vector2) {
        return new Vector2(a.x * b, a.y * b);
      }
    }
  }

  /*---------------------------------------------------------
  * Divide a vector by a scalar
  * @param a - The vector to divide
  * @param b - The scalar to divide by
  * @return type of parameter a - Vector2 or PositionState
  ----------------------------------------------------------*/
  public static Divide(a: Vector2 | Vector2Schema, b: number): Vector2 | Vector2Schema
  {
    if (a instanceof Vector2Schema) {
      return new Vector2Schema(a.x / b, a.y / b);
    } else if (a instanceof Vector2) {
      return new Vector2(a.x / b, a.y / b);
    }
  }

  /*---------------------------------------------------------
  * Compare two vectors
  * @param a - The first vector or postion state
  * @param b - The second vector or position state
  * @return boolean - True if the vectors are equal
  * @return boolean - False if the vectors are not equal
  ---------------------------------------------------------*/
  public static Compare(a: Vector2 | Vector2Schema, b: Vector2 | Vector2Schema): boolean
  {
    return a.x === b.x && a.y === b.y;
  }

  public static Destructure(vector: Vector2 | Vector2Schema): { x: number, y: number }
  {
    return { x: vector.x, y: vector.y };
  }

  public static Convert(position: Vector2Schema): Vector2
  {
    return new Vector2(position.x, position.y);
  }

}
