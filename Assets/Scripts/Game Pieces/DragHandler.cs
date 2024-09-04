using UnityEngine;

/*-----------------------------------------------------------------------------
 * This class is responsible for handling the dragging state of a game piece
 ------------------------------------------------------------------------------*/
public class DragHandler : MonoBehaviour
{
  public DraggableUIGamePiece UIGamePiece { get; set; } //The UI game piece I'm being dragged from
  public GamePieceType TypeOfPiece { get; set; } //The type of game piece
  private bool _isDragging = true; //True if the piece is being dragged
  private GameboardManager _gameboardManager; //Reference to the gameboard manager

  // Start is called before the first frame update
  private void Start()
  {
    _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>(); //Get a reference to the GameboardManager
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
    if (!_isDragging)
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

    PlacedPiece gamePiece = gameObject.AddComponent<PlacedPiece>();

    gamePiece.TypeOfPiece = TypeOfPiece;

    gamePiece.SetTilePlacement(gameTile);
    gamePiece.SendPlacement();
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
        _isDragging = false;
        UIGamePiece.IsDragging = false;
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