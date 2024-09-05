using UnityEngine;

public class PlacedPiece : MonoBehaviour
{
  public GamePieceType TypeOfPiece { set; get; } //The type of game piece
  public GameTile GameTilePlacement { private set; get; } //The game tile the game piece is placed on

  /*-----------------------------------------------------------
   * Set the tile placement of the game piece
   * @param gameTile - The game tile to place the game piece on
   ------------------------------------------------------------*/
  public void SetTilePlacement(GameTile gameTile)
  {
    GameTilePlacement = gameTile;
    gameTile.CurrentlyHeldPiece = this;
    Destroy(GetComponent<DragHandler>());
  }

  
}
