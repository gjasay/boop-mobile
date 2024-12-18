import { GameState, PlayerState } from "../schema/GameState";
import { GameUtils } from "./GameUtils";

export class Timer {
  private timer: NodeJS.Timeout | null = null;
  private player: PlayerState | null = null;
  private gameState: GameState | null = null;
  private startTime: number = 0;

  constructor(player: PlayerState, gameState: GameState) {
    this.player = player;
    this.gameState = gameState;
  }

  start(): void {
    this.startTime = Date.now();
    this.timer = setInterval(() => {
      const elapsed = (Date.now() - this.startTime) / 1000;
      this.player.timer -= elapsed;

      if (this.player.timer <= 0) {
        console.log("Player " + this.player.id + " ran out of time!");
        GameUtils.delcareWinner(this.gameState, this.player.id === 1 ? 2 : 1);
        this.stop();
      }

      this.startTime = Date.now();
    }, 10);
  }

  stop(): void {
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }
}
