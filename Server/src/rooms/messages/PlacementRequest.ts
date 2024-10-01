import e from "express";
import { GamePieceState, GameState, PositionState, TileState } from "../schema/GameState";
import { Vector2 } from "../utils/Vector2";

interface TileNeighbor
{
  tile: TileState;
  direction: Vector2;
}

/*------------------------------------------------------------------
* Handle a request to place a piece on the board
* @param x: The x array position of the tile to place the piece on
* @param y: The y array position of the tile to place the piece on
* @param pieceType: The type of piece to place
* @param playerId: The ID of the player placing the piece
--------------------------------------------------------------------*/
export function handlePlacementRequest(state: GameState, x: number, y: number, pieceType: string, playerId: number)
{

  const tile = state.board.tiles.find((tile) => tile.position.x === x && tile.position.y === y); //Retrieve the tile at the specified coordinates
  const player = playerId === 1 ? state.playerOne : playerId === 2 ? state.playerTwo : null;

  if (state.currentPlayer === null) {
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

  if (playerId !== state.currentPlayer) {
    console.warn("Invalid move: It is not this player's turn.");
    return;
  }

  if (pieceType === "tadpole") {
    if (player.hand.tadpoles <= 0) {
      console.warn("Invalid move: Player does not have any tadpoles left.");
      return;
    }

    setPieceOnTile(state, tile, playerId, pieceType);
    player.hand.tadpoles--;
  }
  else if (pieceType === "frog") {
    if (player.hand.frogs <= 0) {
      console.warn("Invalid move: Player does not have any frogs left.");
      return;
    }

    setPieceOnTile(state, tile, playerId, pieceType);
    player.hand.frogs--;
  }

  state.currentPlayer = playerId === 1 ? 2 : playerId === 2 ? 1 : null;
}

/*---------------------------------------------------------
* Place a piece on a tile
* @param tile: The tile to place the piece on
* @param playerId: The ID of the player placing the piece
* @param type: The type of piece to place
*---------------------------------------------------------*/
function setPieceOnTile(state: GameState, tile: TileState, playerId: number, type: string)
{

  let piece = new GamePieceState(type, playerId);
  tile.gamePiece = piece;

  //Get the first neighbors of the tile

  const tilesToCheck: TileState[] = [tile];

  const neighborsOfNeighbors = [
    getTile(state, Vector2.Add(tile.neighbor.up, Vector2.UP)),
    getTile(state, Vector2.Add(tile.neighbor.down, Vector2.DOWN)),
    getTile(state, Vector2.Add(tile.neighbor.left, Vector2.LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.right, Vector2.RIGHT)),
    getTile(state, Vector2.Add(tile.neighbor.upLeft, Vector2.UP_LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.upRight, Vector2.UP_RIGHT)),
    getTile(state, Vector2.Add(tile.neighbor.downLeft, Vector2.DOWN_LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.downRight, Vector2.DOWN_RIGHT))
  ];

  neighborsOfNeighbors.forEach((neighbor) =>
  {
    if (neighbor !== null) {
      tilesToCheck.push(neighbor);
    }
  });

  console.log(JSON.stringify(neighborsOfNeighbors.map((neighbor) => neighbor?.position)));
  pushTileNeighbors(state, tile);
  checkForRow(state, tilesToCheck, 3);
}

function pushTileNeighbors(state: GameState, tile: TileState)
{
  if (tile === null) return;

  tile.neighbors.forEach((neighbor) =>
  {
    if (neighbor === null) return;

    const neighborTile = getTile(state, neighbor);
    if (neighborTile === null || neighborTile.gamePiece === null) return;

    const direction = Vector2.Subtract(neighborTile.position, tile.position) as Vector2;
    const destinationPosition = Vector2.Add(neighborTile.position, direction);

    if (destinationPosition.x < 0 || destinationPosition.x >= state.board.width || destinationPosition.y < 0 || destinationPosition.y >= state.board.height) {
      if (neighborTile.gamePiece === null) return;

      switch (neighborTile.gamePiece.type) {
        case "tadpole":
          if (neighborTile.gamePiece.playerId === 1) {
            state.playerOne.hand.tadpoles++;
          } else if (neighborTile.gamePiece.playerId === 2) {
            state.playerTwo.hand.tadpoles++;
          } else {
            console.error("Invalid player ID.");
          }
          break;
        case "frog":
          if (neighborTile.gamePiece.playerId === 1) {
            state.playerOne.hand.frogs++;
          } else if (neighborTile.gamePiece.playerId === 2) {
            state.playerTwo.hand.frogs++;
          } else {
            console.error("Invalid player ID.");
          }
          break;
      }

      neighborTile.gamePiece = null;
    };
    const destinationTile = getTile(state, Vector2.Add(neighborTile.position, direction));

    if (!destinationTile) return;

    if (tile.gamePiece?.type === "tadpole" && neighborTile.gamePiece?.type === "frog") return; // Prevent tadpoles from pushing frogs
        
    if (destinationTile.gamePiece === null) {
      destinationTile.gamePiece = neighborTile.gamePiece;
      neighborTile.gamePiece = null;
    }
  })
}

/*---------------------------------------------------------
* Check for rows of three of the same type
* @param state: The current game state
* @param tiles: The tiles to check for three in a row
----------------------------------------------------------*/
function checkForRow(state: GameState, tiles: TileState[], length: number): void
{
  const rowsToTest: Vector2[][] = [
    [Vector2.UP, Vector2.DOWN],
    [Vector2.LEFT, Vector2.RIGHT],
    [Vector2.UP_LEFT, Vector2.DOWN_RIGHT],
    [Vector2.UP_RIGHT, Vector2.DOWN_LEFT],
    [Vector2.UP, Vector2.Multiply(Vector2.UP, 2) as Vector2],
    [Vector2.DOWN, Vector2.Multiply(Vector2.DOWN, 2) as Vector2],
    [Vector2.LEFT, Vector2.Multiply(Vector2.LEFT, 2) as Vector2],
    [Vector2.RIGHT, Vector2.Multiply(Vector2.RIGHT, 2) as Vector2],
    [Vector2.UP_LEFT, Vector2.Multiply(Vector2.UP_LEFT, 2) as Vector2],
    [Vector2.UP_RIGHT, Vector2.Multiply(Vector2.UP_RIGHT, 2) as Vector2],
    [Vector2.DOWN_LEFT, Vector2.Multiply(Vector2.DOWN_LEFT, 2) as Vector2],
    [Vector2.DOWN_RIGHT, Vector2.Multiply(Vector2.DOWN_RIGHT, 2) as Vector2],
  ]

  tiles.forEach((tile) =>
  {
    if (tile === null || tile === undefined) return;
    if (tile.gamePiece === null) return;

    rowsToTest.forEach((row) =>
    {
      const rowPieces: TileState[] = [tile];

      row.forEach((direction) =>
      {
        const neighbor = getTile(state, Vector2.Add(tile.position, direction));

        if (neighbor === null || neighbor === undefined) return;
        if (neighbor.gamePiece === null || tile.gamePiece === null) return;
        if (neighbor.gamePiece.type !== tile.gamePiece.type || neighbor.gamePiece.playerId !== tile.gamePiece.playerId) return;

        rowPieces.push(neighbor);
      });

      if (rowPieces.length >= length) {
        console.log(`Row of ${length} found!`);

        switch (tile.gamePiece.type) {
          case "tadpole":
            if (tile.gamePiece.playerId === 1) {
              state.playerOne.hand.frogs += 3;
            } else if (tile.gamePiece.playerId === 2) {
              state.playerTwo.hand.frogs += 3;
            } else {
              console.error("Invalid player ID.");
            }
            break;
          case "frog":
            state.winner = tile.gamePiece.playerId;
            break;
        }
        rowPieces.forEach((rowPiece) =>
        {
          rowPiece.gamePiece = null;
        });
      }
    });
  })
}

/*---------------------------------------------------------
* Get a tile from the board
* @param state: The current game state
* @param position: The position of the tile to get
* @returns The tile at the specified position or null
----------------------------------------------------------*/
function getTile(state: GameState, position: PositionState | Vector2): TileState | null
{
  if (position === null || position === undefined) return;

  const tile = state.board.tiles[position.y * state.board.width + position.x];

  if (tile === undefined || tile === null) {
    console.warn("Tile does not exist.");
    return null;
  }

  console.log(`Getting tile at: (${tile.position.x}, ${tile.position.y})`);
  return tile;
}
