using UnityEngine;

public class DragHandler : MonoBehaviour
{
  public DraggableUIGamePiece UIGamePiece { get; set; }  // The UI game piece I'm being dragged from
  public GamePieceType TypeOfPiece { get; set; }         // The type of game piece
  public string PieceType { get; set; }                  // The type of game piece as a string
  private GameboardManager _gameboardManager;            // Reference to the gameboard manager
  private MainUIEventHandler _uiManager;                 // Reference to the main UI event handler

  
  private void Start()
  {
    _gameboardManager = GameboardManager.Instance;
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();

    switch(TypeOfPiece)
    {
      case GamePieceType.OrangeKitten:
      PieceType = "kitten";
      break;
      case GamePieceType.GrayKitten:
      PieceType = "kitten";
      break;
      case GamePieceType.OrangeCat:
      PieceType = "cat";
      break;
      case GamePieceType.GrayCat:
      PieceType = "cat";
      break;
    }
  }

  void Update()
  {
    UpdateCurrentPosition();
    CheckForDestruction();
  }

  private void UpdateCurrentPosition()
  {
    if (!UIGamePiece.IsDragging) {
      SetPieceOnTile();
    }

    Vector2? touchPosition = CalculatePosition();

    if (touchPosition != null) {
      transform.position = new Vector3(touchPosition.Value.x, touchPosition.Value.y, 0);
    }
  }

  private void CheckForDestruction()
  {
    Vector2? touchPosition = CalculatePosition();

    if (touchPosition == null) {
      Destroy(gameObject);
    }
  }

  private void SetPieceOnTile()
  {
    GameTile gameTile = _gameboardManager.LastTouchedGameTile;

    // if (gameTile == null || !_gameboardManager.CurrentlyTouchingGameBoard) {
    //   Destroy(gameObject);
    //   return;
    // }

    // transform.position = gameTile.transform.position;

    // if (_uiManager.CanPlacePiece(PieceType)) {
    //   gameTile.PlacePiece(NetworkManager.Instance.PlayerId, PieceType);
    //   gameTile.CurrentlyHeldPiece.clientPlacement = true;
    // }

    SendTilePlacement(gameTile); 

    Destroy(gameObject);
  }

  private void SendTilePlacement(GameTile gameTile)
  {
    NetworkManager networkManager = NetworkManager.Instance;

    if (TypeOfPiece == GamePieceType.OrangeKitten || TypeOfPiece == GamePieceType.GrayKitten) {
      networkManager.PlacePiece(gameTile.ArrayPosition.x, gameTile.ArrayPosition.y, "kitten");
    } else {
      networkManager.PlacePiece(gameTile.ArrayPosition.x, gameTile.ArrayPosition.y, "cat");
    }
  }

  private Vector2? CalculatePosition()
  {
    if (Input.touchCount > 0) {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Ended) {
        UIGamePiece.IsDragging = false;
        return GetNearestGameTilePosition();
      }

      return Camera.main.ScreenToWorldPoint(touch.position);
    }
    return GetNearestGameTilePosition();
  }

  private Vector2? GetNearestGameTilePosition()
  {
    GameTile gameTile = _gameboardManager.LastTouchedGameTile;

    if (gameTile == null || !_gameboardManager.CurrentlyTouchingGameBoard)
      return null;

    Debug.Log("Game tile position: " + gameTile.transform.position);

    return gameTile.transform.position;
  }
}
