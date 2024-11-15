import { GameState, PlayerState } from "../schema/GameState";
import { GameUtils } from "./GameUtils";

export class Timer
{
  private timer: NodeJS.Timeout | null = null;
  private player: PlayerState | null = null;
  private gameState: GameState | null = null;

  constructor(player: PlayerState, gameState: GameState)
  {
    this.player = player;
    this.gameState = gameState;
  }
  
  start(): void
  {
    this.timer = setInterval(() => {
      this.player.timer -= 0.01;
      console.log("Player " + this.player.id + " time: " + this.player.timer);

      if (this.player.timer <= 0) {
        console.log("Player " + this.player.id + " ran out of time!");
        GameUtils.delcareWinner(this.gameState, this.player.id === 1 ? 2 : 1);
        clearInterval(this.timer);
      }
    }, 10);
  }

  stop(): void
  {
    clearInterval(this.timer);
  }
}