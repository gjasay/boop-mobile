import { Room, Client } from "@colyseus/core";
import { GameState } from "./schema/GameState";
import { initializeRoom } from "./messages/CreateGameboard";
import { handlePlacementRequest } from "./messages/PlacementRequest";
import { requestEvolution } from "./messages/EvolutionRequest";
import {Queue} from "./utils/Queue";
import { Timer } from "./utils/Timer";

export class MyRoom extends Room<GameState>
{
  public maxClients = 2;
  public animationQueue: Queue = new Queue();
  public hostClient: Client | null = null;
  public roomInitialized = false;
  public playerOneTimer: Timer | null = null;
  public playerTwoTimer: Timer | null = null;

  onCreate(options: { time: number })
  {
    this.setState(new GameState());

    initializeRoom(this, options);
    this.onMessage("placePiece", (client: Client, msg: PieceMessage) => handlePlacementRequest(this, client, msg));
    this.onMessage("evolveTadpole", (_client: Client, msg: EvolutionMessage) => requestEvolution(this, msg));
  }

  sendEvolutionMessage(client: Client)
  {
    client.send("choosePieceToEvolve");
  }

  /*-------------Colyseus Room Lifecycle Functions-----------------*/
  onJoin(client: Client, options: { time: number })
  {
    console.log(client.sessionId, "joined!");

    if (this.clients.length === 1) {
      this.hostClient = client;
      client.send("playerId", 1);
    }

    if (this.clients.length === this.maxClients && this.hostClient) {
      client.send("playerId", 2);
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
