import { Delayed } from "colyseus";
import { MyRoom } from "../MyRoom";
import { PlayerState } from "../schema/GameState";
import { GameUtils } from "./GameUtils";

export class Timer {
  private _timer: Delayed;
  private _player: PlayerState;
  private _room: MyRoom;
  private _startTime: number = 10;
  private _initalized: boolean = false;

  constructor(player: PlayerState, room: MyRoom) {
    this._player = player;
    this._room = room;
    this.initialize();
  }

  public resume(): void {
    if (this._timer) {
      this._timer.resume();
    }
  }

  public pause(): void {
    if (this._timer) {
      this._timer.pause();
    }
  }

  public clear(): void {
    if (this._timer) {
      this._timer.clear();
    }
  }

  private initialize(): void {
    if (!this._player || !this._room) { throw new Error("Player or room is not initialized!"); }

    this._timer = this._room.clock.setInterval(() => {
      if (!this._initalized) {
        this._timer.pause();
        this._initalized = true;
        this._player.timer = this._startTime;
        return;
      }

      this._player.timer--;
      console.log(`Player ${this._player.id}: ${this._player.timer}`);

      if (this._player.timer <= 0) {
        console.log("Player " + this._player.id + " ran out of time!");
        GameUtils.delcareWinner(this._room, this._player.id === 1 ? 2 : 1);
        this.clear();
      }
    }, 1000);
  }
}
