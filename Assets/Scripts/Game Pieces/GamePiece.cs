using UnityEngine;

public class GamePiece : MonoBehaviour
{
  public GameTile GameTilePlacement { private set; get; } //The game tile the game piece is placed on
  private NetworkManager _networkManager; //Reference to the GameboardManager
  private void Start()
  {
    _networkManager = NetworkManager.Instance;

    SendPlacement();
  }

  public void SetTilePlacement(GameTile gameTile)
  {
    GameTilePlacement = gameTile;
    gameTile.CurrentlyHeldPiece = this;
    Destroy(GetComponent<PieceDragging>());
  }

  private void SendPlacement()
  {
    _networkManager.SendTadpolePlacement(new GamePieceState
    {
      position = { x = GameTilePlacement.transform.position.x, y = GameTilePlacement.transform.position.y },
      playerId = _networkManager.PlayerId,
    });
  }

}
