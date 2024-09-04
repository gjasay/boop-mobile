import { Room, Client } from "@colyseus/core";
import { GamePieceState, GameState } from "./schema/GameState";

export class MyRoom extends Room<GameState> {
  maxClients = 2;

  //Called when room is initialized
  onCreate (options: any) {
    this.setState(new GameState()); //Set the initial state of the room

    /*---------------------------------------------------------------
     * Place a tadpole on the board
     * message = { position: { x: float, y: float }, playerId: int }
    -----------------------------------------------------------------*/
    this.onMessage("placeTadpole", (client, message: GamePieceState) => {
      console.log(client.sessionId, "placed a tadpole at", message.position.x, ", ", message.position.y);

      const tadpoleState = new GamePieceState();

      tadpoleState.position.x = message.position.x;
      tadpoleState.position.y = message.position.y;
      tadpoleState.playerId = message.playerId;

      this.state.board.tadpoles.push(tadpoleState);

      this.broadcast("tadpolePlaced", message);
    });
  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }
}
