using UnityEngine;

public class PlacedPiece : MonoBehaviour
{
  public string pieceType { set; get; }
  public GameTile GameTilePlacement { private set; get; } //The game tile the game piece is placed on

  /*-----------------------------------------------------------
   * Set the tile placement of the game piece
   * @param gameTile - The game tile to place the game piece on
   ------------------------------------------------------------*/
  public void SetTilePlacement(GameTile gameTile, string pieceType)
  {
    this.pieceType = pieceType;
    GameTilePlacement = gameTile;
    gameTile.CurrentlyHeldPiece = this;
    Destroy(GetComponent<DragHandler>());
  } 
}
