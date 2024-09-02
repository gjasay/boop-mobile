using UnityEngine;

public class GamePiece : MonoBehaviour
{
  public GameTile GameTilePlacement { set; get; } //The game tile the game piece is placed on
  private GameboardManager _gameboardManager; //Reference to the GameboardManager
  private void Start()
  {
    _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>(); //Get a reference to the GameboardManager

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
    _gameboardManager.SendTadpolePlacement(new GamePieceState
    {
      position = { x = GameTilePlacement.transform.position.x, y = GameTilePlacement.transform.position.y },
      playerId = _gameboardManager.PlayerId,
    });
  }

}
