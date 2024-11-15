import { Room, Client } from "@colyseus/core";
import { GameState } from "./schema/GameState";
import { initializeRoom } from "./messages/CreateGameboard";
import { handlePlacementRequest } from "./messages/PlacementRequest";
import { requestEvolution } from "./messages/EvolutionRequest";
import { handlePieceMoved } from "./messages/PieceMove";
import { GameUtils } from "./utils/GameUtils";

export class MyRoom extends Room<GameState>
{
  maxClients = 2;
  private hostClient: Client | null = null;

  //Called when room is initialized
  onCreate(options: any)
  {
    this.setState(new GameState());

    this.onMessage("createRoom", (_client: Client, msg: RoomSettings) => initializeRoom(this.state, msg));
    this.onMessage("placePiece", (client: Client, msg: PieceMessage) => handlePlacementRequest(this, client, msg));
    this.onMessage("pieceMoved", (_client: Client, msg: Coordinate) => handlePieceMoved(this.state, msg));
    this.onMessage("evolveTadpole", (_client: Client, msg: EvolutionMessage) => requestEvolution(this, msg));
  }

  sendEvolutionMessage(client: Client)
  {
    client.send("choosePieceToEvolve");
  }

  /*-------------Colyseus Room Lifecycle Functions-----------------*/
  onJoin(client: Client, options: any)
  {
    console.log(client.sessionId, "joined!");

    if (this.clients.length === 1) {
      this.hostClient = client;
    }

    if (this.clients.length === this.maxClients && this.hostClient) {
      this.hostClient.send("playerJoined");

      GameUtils.startPlayerTimer(this.state, this.state.playerOne);
    }
  }

  onLeave(client: Client, consented: boolean)
  {
    console.log(client.sessionId, "left!");
  }

  onDispose()
  {
    console.log("room", this.roomId, "disposing...");
  }
}
