using UnityEngine;

/*-----------------------------------------------------------------------------
 * This class is responsible for handling the dragging state of a game piece
 ------------------------------------------------------------------------------*/
public class DragHandler : MonoBehaviour
{
  public DraggableUIGamePiece UIGamePiece { get; set; } //The UI game piece I'm being dragged from
  public GamePieceType TypeOfPiece { get; set; } //The type of game piece
  public string PieceType { get; set; } //The type of game piece as a string
  private GameboardManager _gameboardManager; //Reference to the gameboard manager
  private MainUIEventHandler _uiManager; //Reference to the main UI event handler

  // Start is called before the first frame update
  private void Start()
  {
    _gameboardManager = GameboardManager.Instance;
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();
  }
  //Update is called once per frame
  void Update()
  {
    UpdateCurrentPosition();
    CheckForDestruction();
  }

  /*--------------------------------------------------------------------------------
   * Update the current position
   * This method is responsible for updating the current position of the game piece
   ---------------------------------------------------------------------------------*/
  private void UpdateCurrentPosition()
  {
    if (!_uiManager.IsDragging)
    {
      SetPieceOnTile();
    }

    Vector2? touchPosition = CalculatePosition();

    if (touchPosition != null)
    {
      transform.position = new Vector3(touchPosition.Value.x, touchPosition.Value.y, 0);
    }
  }

  /*--------------------------------------------------------------
   * Will destroy the game piece if the touch position is invalid
   ---------------------------------------------------------------*/
  private void CheckForDestruction()
  {
    Vector2? touchPosition = CalculatePosition();

    if (touchPosition == null)
    {
      Destroy(gameObject);
    }
  }

  /*------------------------------------------------------------------------------------------------
   * Set piece on tile
   * This method is responsible for checking if the tile is empty and setting the piece on the tile
   -------------------------------------------------------------------------------------------------*/
  private void SetPieceOnTile()
  {
    GameTile gameTile = _gameboardManager.LastTouchedGameTile;

    if (gameTile == null || gameTile.CurrentlyHeldPiece != null)
    {
      Destroy(gameObject);
      return;
    }
    transform.position = gameTile.transform.position;

    // if (_uiManager.CanPlacePiece(PieceType)) 
    // {
    //   _gameboardManager.PlacePiece(gameTile.ArrayPosition.x, gameTile.ArrayPosition.y, NetworkManager.Instance.PlayerId, PieceType); //Place the piece on the game board
    // }

    SendTilePlacement(gameTile); //Send the placement of the game piece to the server

    Destroy(gameObject); //Destroy the game piece
  }

  /*---------------------------------------------------------
  * Send the placement of the game piece to the server
  * @param gameTile - The game tile the game piece is placed on
  ----------------------------------------------------------*/
  private void SendTilePlacement(GameTile gameTile)
  {
    NetworkManager networkManager = NetworkManager.Instance;

    if (TypeOfPiece == GamePieceType.OrangeTadpole || TypeOfPiece == GamePieceType.PurpleTadpole)
    {
      networkManager.PlacePiece(gameTile.ArrayPosition.x, gameTile.ArrayPosition.y, "tadpole");
    }
    else
    {
      networkManager.PlacePiece(gameTile.ArrayPosition.x, gameTile.ArrayPosition.y, "frog");
    }
  }

  /*--------------------------------------------------------------------------------------
   * Calculate the position for the game piece in this drag state
   * @return Vector2 - The touch position while dragging
   * @return Vector2? - Game tile position (or null) if the player is finished dragging
   --------------------------------------------------------------------------------------*/
  private Vector2? CalculatePosition()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Ended)
      {
        _uiManager.IsDragging = false;
        _uiManager.SetUIGamePieces();
        return GetNearestGameTilePosition();
      }

      return Camera.main.ScreenToWorldPoint(touch.position);
    }
    return GetNearestGameTilePosition();
  }

  /*---------------------------------------------
   * Get the position of the nearest game tile
   * @return Vector2 - The position of the nearest game tile
   * @return null - If the player is not touching the game board
   ----------------------------------------------*/
  private Vector2? GetNearestGameTilePosition()
  {
    GameTile gameTile = _gameboardManager.LastTouchedGameTile;

    if (gameTile == null || !_gameboardManager.CurrentlyTouchingGameBoard) return null;

    return gameTile.transform.position;
  }
}