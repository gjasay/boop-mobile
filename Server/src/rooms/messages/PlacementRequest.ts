import { GamePieceState, GameState, PositionState, TileState } from "../schema/GameState";
import { Vector2 } from "../utils/Vector2";

/*------------------------------------------------------------------
* Handle a request to place a piece on the board
* @param x: The x array position of the tile to place the piece on
* @param y: The y array position of the tile to place the piece on
* @param pieceType: The type of piece to place
* @param playerId: The ID of the player placing the piece
--------------------------------------------------------------------*/
export function handlePlacementRequest(state: GameState, x: number, y: number, pieceType: string, playerId: number): void {
  const tile = getTile(state, new PositionState(x, y));
  const player = getPlayer(state, playerId);

  if (!isValidMove(state, tile, player, playerId)) return;

  placePiece(state, tile, player, pieceType);
  switchPlayer(state, playerId);
}

/*---------------------------------------------------------
* Get a player from the game state
* @param state: The current game state
* @param playerId: The ID of the player to get
* @returns The player with the specified ID or null
----------------------------------------------------------*/
function getPlayer(state: GameState, playerId: number) {
  return playerId === 1 ? state.playerOne : playerId === 2 ? state.playerTwo : null;
}

/*---------------------------------------------------------
* Check if a move is valid
* @param state: The current game state
* @param tile: The tile to place the piece on
* @param player: The player placing the piece
* @param playerId: The ID of the player placing the piece
* @returns True if the move is valid, false otherwise
----------------------------------------------------------*/
function isValidMove(state: GameState, tile: TileState | null, player: any, playerId: number): boolean {
  if (!state.currentPlayer) {
    console.error("Current player is null.");
    return false;
  }

  if (!tile || tile.gamePiece) {
    console.warn("Invalid move: Tile is occupied or does not exist.");
    return false;
  }

  if (!player) {
    console.warn("Invalid move: Player does not exist.");
    return false;
  }

  if (playerId !== state.currentPlayer) {
    console.warn("Invalid move: It is not this player's turn.");
    return false;
  }

  return true;
}

/*---------------------------------------------------------
* Place a piece on the board
* @param tile: The tile to place the piece on
* @param player: The player placing the piece
* @param pieceType: The type of piece to place
----------------------------------------------------------*/
function placePiece(state: GameState, tile: TileState, player: any, pieceType: string): void {
  if (pieceType === "tadpole" && player.hand.tadpoles > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.tadpoles--;
  } else if (pieceType === "frog" && player.hand.frogs > 0) {
    setPieceOnTile(state, tile, player.id, pieceType);
    player.hand.frogs--;
  } else {
    console.warn(`Invalid move: Player does not have any ${pieceType}s left.`);
  }
}

function switchPlayer(state: GameState, playerId: number): void {
  state.currentPlayer = playerId === 1 ? 2 : playerId === 2 ? 1 : null;
}

/*---------------------------------------------------------
* Place a piece on a tile
* @param tile: The tile to place the piece on
* @param playerId: The ID of the player placing the piece
* @param type: The type of piece to place
----------------------------------------------------------*/
function setPieceOnTile(state: GameState, tile: TileState, playerId: number, type: string): void {
  tile.gamePiece = new GamePieceState(type, playerId); // Place the piece on the tile

  //Process game logic
  pushTileNeighbors(state, tile);

  const tilesToCheck: TileState[] = [tile];
  const extendedNeighbors = getExtendedNeighbors(state, tile);

  extendedNeighbors.forEach((neighbor) => {
    if (neighbor) tilesToCheck.push(neighbor);
  });

  checkForRow(state, tilesToCheck);
}

/*---------------------------------------------------------
* Get the neighbors two tiles away from the specified tile
* @param state: The current game state
* @param tile: The tile to get the neighbors of
* @returns An array of the neighbors two tiles away
----------------------------------------------------------*/
function getExtendedNeighbors(state: GameState, tile: TileState): (TileState | null)[] {
  return [
    getTile(state, Vector2.Add(tile.neighbor.up, Vector2.UP)),
    getTile(state, Vector2.Add(tile.neighbor.down, Vector2.DOWN)),
    getTile(state, Vector2.Add(tile.neighbor.left, Vector2.LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.right, Vector2.RIGHT)),
    getTile(state, Vector2.Add(tile.neighbor.upLeft, Vector2.UP_LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.upRight, Vector2.UP_RIGHT)),
    getTile(state, Vector2.Add(tile.neighbor.downLeft, Vector2.DOWN_LEFT)),
    getTile(state, Vector2.Add(tile.neighbor.downRight, Vector2.DOWN_RIGHT))
  ];
}

/*---------------------------------------------------------
* Push the neighbors of a tile one tile away from the tile
* @param state: The current game state
* @param tile: The tile to push the neighbors of
----------------------------------------------------------*/
function pushTileNeighbors(state: GameState, tile: TileState): void {
  if (!tile) return;

  tile.neighbors.forEach((neighbor) => {
    if (!neighbor) return;

    const neighborTile = getTile(state, neighbor);
    if (!neighborTile || !neighborTile.gamePiece) return;

    const direction = Vector2.Subtract(neighborTile.position, tile.position) as Vector2;
    const destinationPosition = Vector2.Add(neighborTile.position, direction);

    if (isOutOfBounds(state, destinationPosition)) {
      handleOutOfBounds(state, neighborTile);
      return;
    }

    const destinationTile = getTile(state, destinationPosition);
    if (!destinationTile) return;

    if (tile.gamePiece?.type === "tadpole" && neighborTile.gamePiece?.type === "frog") return;

    if (!destinationTile.gamePiece) {
      destinationTile.gamePiece = neighborTile.gamePiece;
      neighborTile.gamePiece = null;
    }
  });
}

/*--------------------------------------------------------------
* Check if an array position is outside the bounds of the board
* @param state: The current game state
* @param position: The position to check
---------------------------------------------------------------*/
function isOutOfBounds(state: GameState, position: Vector2): boolean {
  return position.x < 0 || position.x >= state.board.width || position.y < 0 || position.y >= state.board.height;
}

/*----------------------------------------------------------
* Handle a piece being pushed out of bounds
* @param state: The current game state
* @param tile: The tile that the piece is being pushed from
-----------------------------------------------------------*/
function handleOutOfBounds(state: GameState, tile: TileState): void {
  if (!tile.gamePiece) return;

  switch (tile.gamePiece.type) {
    case "tadpole":
      if (tile.gamePiece.playerId === 1) {
        state.playerOne.hand.tadpoles++;
      } else if (tile.gamePiece.playerId === 2) {
        state.playerTwo.hand.tadpoles++;
      } else {
        console.error("Invalid player ID.");
      }
      break;
    case "frog":
      if (tile.gamePiece.playerId === 1) {
        state.playerOne.hand.frogs++;
      } else if (tile.gamePiece.playerId === 2) {
        state.playerTwo.hand.frogs++;
      } else {
        console.error("Invalid player ID.");
      }
      break;
  }

  tile.gamePiece = null;
}

/*---------------------------------------------------------
* Check for rows of three of the same type
* @param state: The current game state
* @param tiles: The tiles to check for three in a row
----------------------------------------------------------*/
function checkForRow(state: GameState, tiles: TileState[]): void {
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
  ];

  tiles.forEach((tile) => {
    if (!tile || !tile.gamePiece) return;

    rowsToTest.forEach((row) => {
      const rowPieces: TileState[] = [tile];

      row.forEach((direction) => {
        const neighbor = getTile(state, Vector2.Add(tile.position, direction));

        if (!neighbor || !neighbor.gamePiece || !tile.gamePiece) return;
        if (neighbor.gamePiece.playerId !== tile.gamePiece.playerId) return;

        rowPieces.push(neighbor);
      });

      if (rowPieces.length === 3) {
        handleRowFound(state, tile, rowPieces);
      }
    });
  });
}

/*---------------------------------------------------------
* Handle a row of three pieces being found
* If the pieces are tadpoles, they are removed from the 
* board and three frogs are added to the player's hand
* If the pieces are frogs, the player who placed the frogs win the game
* @param state: The current game state
* @param tile: The tile that was placed
* @param rowPieces: The pieces in the row
----------------------------------------------------------*/
function handleRowFound(state: GameState, tile: TileState, rowPieces: TileState[]): void {
  console.log(`Row of ${rowPieces.length} found!`);
  const player = getPlayer(state, tile.gamePiece.playerId);

  if (rowPieces.every((piece) => piece.gamePiece?.type === "frog")) {
    state.winner = player.id;
  } else {
    rowPieces.forEach((piece) => {
      console.log(`Removing piece at ${piece.position.x}, ${piece.position.y}`);

      player.hand.frogs++;
      piece.gamePiece = null;
    });
  }
}

/*---------------------------------------------------------
* Get a tile from the board
* @param state: The current game state
* @param position: The position of the tile to get
* @returns The tile at the specified position or null
----------------------------------------------------------*/
function getTile(state: GameState, position: PositionState | Vector2): TileState | null {
  if (!position) return null;

  const tile = state.board.tiles[position.y * state.board.width + position.x];

  if (!tile) {
    console.warn("Tile does not exist.");
    return null;
  }

  return tile;
}
