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
    this.onMessage("placeTadpole", (_client, message: GamePieceState) => {
      console.log("placeTadpole", message);
      const tadpoleState = new GamePieceState();

      tadpoleState.tile.x = message.tile.x;
      tadpoleState.tile.y = message.tile.y;
      tadpoleState.playerId = message.playerId;

      this.state.board.tadpoles.push(tadpoleState);

      this.broadcast("tadpolePlaced", message);
    });

    /*--------------------------------------------------------------
    * Place a frog on the board
    * message = { position: { x: float, y: float }, playerId: int }
    * -------------------------------------------------------------*/
    this.onMessage("placeFrog", (_client, message: GamePieceState) => {
      const frogState = new GamePieceState();

      frogState.tile.x = message.tile.x;
      frogState.tile.y = message.tile.y;
      frogState.playerId = message.playerId;

      this.state.board.frogs.push(frogState);

      this.broadcast("frogPlaced", message);
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
