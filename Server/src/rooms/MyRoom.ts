import { Room, Client } from "@colyseus/core";
import { GamePieceState, GameState, TileState } from "./schema/GameState";

interface PieceMessage
{
  x: number;
  y: number;
  type: string;
  playerId: number;
}

export class MyRoom extends Room<GameState>
{
  maxClients = 2;

  //Called when room is initialized
  onCreate(options: any)
  {
    this.setState(new GameState()); //Set the initial state of the room

    this.onMessage("createRoom", this.createGameboard.bind(this));
    this.onMessage("placePiece", (_client: Client, msg: PieceMessage) => this.placePiece(msg.x, msg.y, msg.type, msg.playerId));

  }

  createGameboard()
  {
    const board = this.state.board;
    board.width = 6;
    board.height = 6;

    console.log("Creating gameboard...");
    //Create the gameboard
    for (let i = 0; i < board.width; i++) {
      for (let j = 0; j < board.width; j++) {
        let tile = new TileState();
        tile.x = i;
        tile.y = j;

        this.state.board.tiles.push(tile);
      }
    }
  }

  placePiece(x: number, y: number, pieceType: string, playerId: number)
  {
    console.log("Attempting to place piece at", x, y, "for player", playerId, "...");
    let tile = this.state.board.tiles.find((tile) => tile.x === x && tile.y === y);
    if (tile && tile.gamePiece === null) {
      let piece = new GamePieceState();
      piece.type = pieceType;
      piece.playerId = playerId;
      tile.gamePiece = piece;

      console.log("Piece placed successfully!");
    }
    else {
      console.log("Invalid move!");
      this.broadcast("error", { message: "Invalid move", player: playerId });
    }
  }

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
