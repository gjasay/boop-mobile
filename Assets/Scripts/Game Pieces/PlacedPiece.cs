using UnityEngine;

public class PlacedPiece : MonoBehaviour
{
  public string pieceType { private set; get; }
  public GameTile GameTilePlacement { private set; get; } //The game tile the game piece is placed on
  public bool IsMoving = false; //True if the game piece is moving
  public bool pieceTypeSet = false;

  /*-----------------------------------------------------------
   * Set the tile placement of the game piece
   * @param gameTile - The game tile to place the game piece on
   ------------------------------------------------------------*/
  public void SetTilePlacement(GameTile gameTile)
  {
    GameTilePlacement = gameTile;
    gameTile.CurrentlyHeldPiece = this;
    if (!GetComponent<DragHandler>()) return;
    Destroy(GetComponent<DragHandler>());
  } 

  public void SetPieceType(string pieceType)
  {
    if (pieceTypeSet) return;
    this.pieceType = pieceType;
    pieceTypeSet = true;
  }
}
