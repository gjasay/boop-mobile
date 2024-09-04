using UnityEngine;

public class PlacedPiece : MonoBehaviour
{
  public GamePieceType TypeOfPiece { set; get; } //The type of game piece
  public GameTile GameTilePlacement { private set; get; } //The game tile the game piece is placed on
  private NetworkManager _networkManager; //Reference to the GameboardManager

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

  /*---------------------------------------------------------
   * Send the placement of the game piece to the opponent
   ----------------------------------------------------------*/
  public void SendPlacement()
  {
    _networkManager = NetworkManager.Instance;

    if (TypeOfPiece == GamePieceType.OrangeTadpole || TypeOfPiece == GamePieceType.PurpleTadpole)
    {
      _networkManager.SendTadpolePlacement(new GamePieceState
      {
        tile = { x = GameTilePlacement.ArrayPosition.x, y = GameTilePlacement.ArrayPosition.y },
        playerId = _networkManager.PlayerId,
      });
    }
    else
    {
      _networkManager.SendFrogPlacement(new GamePieceState
      {
        tile = { x = GameTilePlacement.ArrayPosition.x, y = GameTilePlacement.ArrayPosition.y },
        playerId = _networkManager.PlayerId,
      });
    }
  }
}
