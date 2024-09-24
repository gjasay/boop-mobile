import { Room, Client } from "@colyseus/core";
import { GamePieceState, GameState, TileState } from "./schema/GameState";

interface PieceMessage
{
  x: number;
  y: number;
  type: string;
  playerId: number;
}

class Vector2
{
  x: number;
  y: number;

  constructor(x: number, y: number)
  {
    this.x = x;
    this.y = y;
  }
}

export class MyRoom extends Room<GameState>
{
  maxClients = 2;

  //Called when room is initialized
  onCreate(options: any)
  {
    this.setState(new GameState()); //Set the initial state of the room

    this.onMessage("createRoom", this.createGameboard.bind(this));
    this.onMessage("placePiece", (_client: Client, msg: PieceMessage) => this.handlePlacementRequest(msg.x, msg.y, msg.type, msg.playerId));
  }

  /*-------------------------------------
  * Create the gameboard for the room
  ---------------------------------------*/
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

  /*------------------------------------------------------------------
  * Handle a request to place a piece on the board
  * @param x: The x array position of the tile to place the piece on
  * @param y: The y array position of the tile to place the piece on
  * @param pieceType: The type of piece to place
  * @param playerId: The ID of the player placing the piece
  --------------------------------------------------------------------*/
  handlePlacementRequest(x: number, y: number, pieceType: string, playerId: number)
  {

    const tile = this.state.board.tiles.find((tile) => tile.x === x && tile.y === y); //Retrieve the tile at the specified coordinates
    const player = playerId === 1 ? this.state.playerOne : playerId === 2 ? this.state.playerTwo : null;

    if (this.state.currentPlayer === null) {
      console.error("Current player is null.");
      return;
    }

    if (!tile || tile.gamePiece !== null) {
      console.warn("Invalid move: Tile is occupied or does not exist.");
      return;
    }

    if (!player) {
      console.warn("Invalid move: Player does not exist.");
      return;
    }

    if (playerId !== this.state.currentPlayer) {
      console.warn("Invalid move: It is not this player's turn.");
      return;
    }

    if (pieceType === "tadpole") {
      if (player.hand.tadpoles <= 0) {
        console.warn("Invalid move: Player does not have any tadpoles left.");
        return;
      }

      this.setPieceOnTile(tile, playerId, pieceType);
      player.hand.tadpoles--;
    }
    else if (pieceType === "frog") {
      if (player.hand.frogs <= 0) {
        console.warn("Invalid move: Player does not have any frogs left.");
        return;
      }

      this.setPieceOnTile(tile, playerId, pieceType);
      player.hand.frogs--;
    }

    this.state.currentPlayer = playerId === 1 ? 2 : playerId === 2 ? 1 : null;
  }

  /*---------------------------------------------------------
  * Place a piece on a tile
  * @param tile: The tile to place the piece on
  * @param playerId: The ID of the player placing the piece
  * @param type: The type of piece to place
  *---------------------------------------------------------*/
  setPieceOnTile(tile: TileState, playerId: number, type: string)
  {

    let piece = new GamePieceState(type, playerId);
    tile.gamePiece = piece;

    const player = playerId === 1 ? this.state.playerOne : playerId === 2 ? this.state.playerTwo : null;
    //Get the first neighbors of the tile
    const firstNeighbors: TileState[] = this.checkForNeighbors(tile);

    firstNeighbors.forEach((neighbor) =>
    {
      if (neighbor.gamePiece !== null) {
        console.log(`Neighbor at (${neighbor.x}, ${neighbor.y})`);

        const direction = new Vector2(neighbor.x - tile.x, neighbor.y - tile.y);
        const nextTile = this.state.board.tiles.find((t) => t.x === neighbor.x + direction.x && t.y === neighbor.y + direction.y);

        if (!nextTile) return;

        if (nextTile && nextTile.gamePiece === null) {
          nextTile.gamePiece = neighbor.gamePiece;
          neighbor.gamePiece = null;
        }

        /*--------------------------------------------------------------------------------------------------
        * Two differnt possible approaches here to check for three in a row of same piece type
        * I need to check these after the piece has been placed and movement has been resolved
        * Approach 1: The entire gameboard checks for three in a row
        * Approach 2: Only the neighbors of the placed piece and the placed piece check for three in a row
        * which approach is better chatGPT? 
        * I think approach 2 is better because it is more efficient
        * TODO: Create a method to check for three in a row
        * TODO: Implement the method in the setPieceOnTile method
        ----------------------------------------------------------------------------------------------------*/

        if (neighbor.gamePiece?.playerId === nextTile.gamePiece?.playerId && neighbor.gamePiece?.playerId === tile.gamePiece?.playerId) {

          switch (neighbor.gamePiece.type && nextTile.gamePiece.type && tile.gamePiece.type) {
            case "tadpole":
              neighbor.gamePiece = null;
              nextTile.gamePiece = null;
              tile.gamePiece = null;
              player.hand.frogs += 3;
              break;
            case "frog":
              neighbor.gamePiece = null;
              nextTile.gamePiece = null;
              tile.gamePiece = null;
              player.hand.tadpoles += 3;
              break;
          }
        }
      }
    });

    console.log("Piece placed successfully!");
  }

  /*---------------------------------------------------------
  * Check for neighbors of a tile
  * @param tile: The tile to check for neighbors
  * @returns An array of neighboring tiles
  ----------------------------------------------------------*/
  checkForNeighbors(tile: TileState): TileState[] | null
  {
    const directions: Vector2[] = [new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1)];

    return directions.map((dir) =>
    {
      return this.state.board.tiles.find((t) => t.x === tile.x + dir.x && t.y === tile.y + dir.y);
    }).filter((t) => t !== undefined);
  }

  /*---------------------------------------------------------------
  * Check if array of tiles contains the same type of game piece
  * @param tiles: The array of tiles to check
  * @returns True if all tiles contain the same type of game piece
  -----------------------------------------------------------------*/
  checkForSameType(tiles: TileState[]): boolean
  {
    const firstPiece = tiles[0].gamePiece;

    return tiles.every((tile) => tile.gamePiece.type === firstPiece.type);
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
