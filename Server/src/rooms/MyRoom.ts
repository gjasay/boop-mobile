import { Room, Client } from "@colyseus/core";
import { GameState } from "./schema/GameState";
import { createGameboard } from "./messages/CreateGameboard";
import { handlePlacementRequest } from "./messages/PlacementRequest";
import { requestEvolution } from "./messages/EvolutionRequest";

interface PieceMessage
{
  x: number;
  y: number;
  type: string;
  playerId: number;
}

interface EvolutionMessage
{
  x: number;
  y: number;
  playerId: number;
}

export class MyRoom extends Room<GameState>
{
  maxClients = 2;

  //Called when room is initialized
  onCreate(options: any)
  {
    this.setState(new GameState()); //Set the initial state of the room

    this.onMessage("createRoom", () => createGameboard(this.state)); //Create the gameboard for the room
    this.onMessage("placePiece", (client: Client, msg: PieceMessage) => handlePlacementRequest(this, client, msg.x, msg.y, msg.type, msg.playerId));
    this.onMessage("evolveTadpole", (_client: Client, msg: EvolutionMessage) => requestEvolution(this, msg.x, msg.y, msg.playerId));
  }

  sendEvolutionMessage(client: Client)
  {
    client.send("choosePieceToEvolve");
  }

  /*-------------Colyseus Room Lifecycle Functions-----------------*/
  onJoin(client: Client, options: any)
  {
    console.log(client.sessionId, "joined!");
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
