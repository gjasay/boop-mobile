import { MyRoom } from "../MyRoom";
import {
  GameState,
  Vector2Schema,
  TileState,
  PlayerState,
  GamePieceState,
} from "../schema/GameState";
import { Timer } from "./Timer";
import { Vector2 } from "./Vector2";

export class GameUtils {
  public static getPlayer(state: GameState, playerId: number) {
    return playerId === 1
      ? state.playerOne
      : playerId === 2
        ? state.playerTwo
        : null;
  }

  public static getTile(
    state: GameState,
    position: Vector2Schema | Vector2,
  ): TileState | null {
    if (!position) return null;

    const tile = state.tiles[position.y * state.boardSize.x + position.x];

    if (!tile) {
      // console.warn("Tile does not exist.");
      return null;
    }

    return tile;
  }

  public static getPiece(
    state: GameState,
    position: Vector2Schema | TileState | Vector2,
  ): GamePieceState | null {
    if (!position) return null;
    let coordinate: Vector2Schema | Vector2;

    if (position instanceof TileState) {
      coordinate = position.coordinate;
    } else {
      coordinate = position;
    }

    const piece = state.gamePieces.find(
      (piece) =>
        piece.coordinate.x === coordinate.x &&
        piece.coordinate.y === coordinate.y,
    );

    if (!piece) {
      // console.warn("Piece does not exist.");
      return null;
    }

    return piece;
  }

  public static placePiece(
    state: GameState,
    position: Vector2Schema | TileState,
    type: string,
    id: number,
  ): GamePieceState {
    let coordinate: Vector2Schema;

    if (position instanceof TileState) {
      coordinate = position.coordinate;
    } else {
      coordinate = position;
    }

    const piece = state.gamePieces.find(
      (piece) =>
        piece.coordinate.x === -1 &&
        piece.coordinate.y === -1 &&
        piece.playerId === id &&
        piece.type === type,
    );

    if (piece === undefined) {
      console.warn("No piece found in hand.");
      return null;
    }

    const player = this.getPlayer(state, id);

    switch (type) {
      case "kitten":
        player.hand.kittens--;
        break;
      case "cat":
        player.hand.cats--;
        break;
      default:
        console.warn("Invalid piece type.");
        break;
    }

    piece.coordinate = coordinate;
    return piece;
  }

  public static movePiece(
    piece: GamePieceState,
    position: Vector2Schema | TileState | Vector2,
  ): void {
    let coordinate: Vector2Schema;

    if (position instanceof TileState) {
      coordinate = position.coordinate;
    } else {
      if (position instanceof Vector2) {
        coordinate = new Vector2Schema(position.x, position.y);
      } else {
        coordinate = position;
      }
    }

    piece.coordinate = coordinate;
  }

  public static removePiece(state: GameState, piece: GamePieceState): void {
    if (piece.coordinate.x === -1 || piece.coordinate.y === -1) return;
    const player: PlayerState = GameUtils.getPlayer(state, piece.playerId)
    if (piece.isGraduating) {
      player.hand.cats++;
      piece.isGraduating = false;
    } else {
      player.hand.kittens++;
    }
    piece.coordinate = new Vector2Schema();
  }

  public static queueDestruction(room: MyRoom, piece: GamePieceState): void {
    room.animationQueue.increment();
    piece.outOfBounds = "destroy";
  }

  public static isOutOfBounds(state: GameState, position: Vector2): boolean {
    return (
      position.x < 0 ||
      position.x >= state.boardSize.x ||
      position.y < 0 ||
      position.y >= state.boardSize.y
    );
  }

  public static switchPlayer(room: MyRoom, playerId: number): void {
    console.log(
      `[Player ${playerId}] Turn complete. Switching to player ${playerId === 1 ? 2 : 1}...`,
    );

    this.pausePlayerTimer(room, this.getPlayer(room.state, playerId));
    this.startPlayerTimer( room, this.getPlayer(room.state, playerId === 1 ? 2 : 1),
    );
    room.state.currentPlayer = playerId === 1 ? 2 : playerId === 2 ? 1 : null;
  }

  public static delcareWinner(room: MyRoom, playerId: number): void {
    room.state.winner = playerId;
    room.playerOneTimer = null;
    room.playerTwoTimer = null;
    console.log("Player " + playerId + " wins!");
  }

  public static initializePlayerTimers(room: MyRoom): void {
    room.playerOneTimer = new Timer(room.state.playerOne, room);
    room.playerTwoTimer = new Timer(room.state.playerTwo, room);
    this.startPlayerTimer(room, room.state.playerOne);
  }

  private static startPlayerTimer(room: MyRoom, player: PlayerState): void {
    const timer: Timer = player.id === 1 ? room.playerOneTimer : room.playerTwoTimer;

    if (timer != null) {
      timer.resume();
    } else {
      console.warn("Timer is null");
    }
  }

  private static pausePlayerTimer(room: MyRoom, player: PlayerState): void {
    const timer: Timer = player.id === 1 ? room.playerOneTimer : room.playerTwoTimer;

    if (timer != null) {
      timer.pause();
    } else {
      console.warn("Timer is null");
    }
  }
}
