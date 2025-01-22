import {MyRoom} from "../MyRoom";
import {GamePieceState, GameState, PlayerState, Vector2Schema, TileState} from "../schema/GameState";
import {Vector2} from "../utils/Vector2";
import {GameUtils} from "../utils/GameUtils";
import {Client} from "colyseus";
import {handlePostPlacement} from "./PostPlacementLogic";

export async function handlePlacementRequest(room: MyRoom, client: Client, piece: PieceMessage): Promise<void>
{
  const state = room.state;
  const tile = GameUtils.getTile(state, new Vector2Schema(piece.x, piece.y));
  const player = GameUtils.getPlayer(state, piece.playerId);

  console.log(`[Player ${player.id}] Attempting to place ${piece.type} at (${piece.x}, ${piece.y})`);

  if (!isValidMove(state, tile, player)) return;

  if (placePiece(room, tile, player, piece.type))
  {
    handlePostPlacement(room, client, piece)
  }
}

function isValidMove(state: GameState, tile: TileState | null, player: PlayerState): boolean
{
  if (!state.currentPlayer) {
    console.error("Current player is null.");
    return false;
  }

  if (!tile || GameUtils.getPiece(state, tile)) {
    console.warn("Invalid move: Tile is occupied or does not exist.");
    return false;
  }

  if (!player) {
    console.warn("Invalid move: Player does not exist.");
    return false;
  }

  if (player.id !== state.currentPlayer) {
    console.warn("Invalid move: It is not this player's turn.");
    return false;
  }

  return true;
}

function placePiece(room: MyRoom, tile: TileState, player: PlayerState, pieceType: string): boolean
{
  if (pieceType === "kitten" && player.hand.kittens > 0) {
    setPieceOnTile(room, tile, player.id, pieceType);
    return true;
  } else if (pieceType === "cat" && player.hand.cats > 0) {
    setPieceOnTile(room, tile, player.id, pieceType);
    return true;
  } else {
    console.warn(`Invalid move: Player does not have any ${pieceType}s left.`);
    return false;
  }
}

function setPieceOnTile(room: MyRoom, tile: TileState, playerId: number, type: string): void
{
  const state = room.state;

  const piece = GameUtils.placePiece(state, tile, type, playerId);
  
  piece.outOfBounds = null;

  console.log(`[Player ${playerId}] Placed ${type} at (${tile.coordinate.x}, ${tile.coordinate.y})`);

  pushTileNeighbors(room, tile);
}

function pushTileNeighbors(room: MyRoom, tile: TileState): void
{
  if (!tile) return;
  
  const state = room.state;
  const originPiece = GameUtils.getPiece(state, tile);

  console.log(`[Player ${originPiece?.playerId}] Pushing neighbors of (${tile.coordinate.x}, ${tile.coordinate.y})`);

  tile.neighbors.forEach((neighbor) => {
    if (!neighbor) return;

    const neighborPiece = GameUtils.getPiece(state, neighbor);

    if (!neighborPiece) return;
    if (originPiece?.type === "kitten" && neighborPiece?.type === "cat") return; // kittens cannot push cats
    const direction = Vector2.Subtract(neighborPiece.coordinate, originPiece.coordinate) as Vector2;
    const destinationPosition = Vector2.Add(neighborPiece.coordinate, direction);

    if (GameUtils.isOutOfBounds(state, destinationPosition)) {
      console.log(`[Player ${neighborPiece.playerId}] Piece pushed out of bounds at (${neighborPiece.coordinate.x}, ${neighborPiece.coordinate.y})`);
      GameUtils.removePiece(state, neighborPiece);
      return;
    }

    const destinationTile = GameUtils.getTile(state, destinationPosition);
    if (!destinationTile) return;

    if (!GameUtils.getPiece(state, destinationTile)) {
      GameUtils.movePiece(neighborPiece, destinationTile);
    }
  });
}

