using UnityEngine;



public class DraggableUIGamePiece : MonoBehaviour
{
  //Properties
  public bool IsDragging { get; set; } //True if the game piece is being dragged

  private enum DraggerType { Tadpole, Frog } //The type of game piece this dragger is responsible for

  [SerializeField] private DraggerType _draggerType; //The type of game piece that can be dragged

  //Private variables
  private SpriteRenderer _spriteRenderer; //Reference to the sprite renderer
  private ResourceManager _resourceManager; //Reference to the resource manager
  private GamePieceManager _gamePieceManager; //Reference to the game piece manager
  private NetworkManager _networkManager; //Reference to the network manager
  private GameObject _prefab; //Reference to the prefab to be instantiated
  private Sprite _sprite; //The sprite of the game piece

  // Start is called before the first frame update
  void Start()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;
    _networkManager = NetworkManager.Instance;
  }

  // Update is called once per frame
  void Update()
  {
    DragPiece();
  }

  /*-----------------------------------------------
   * Set the UI game pieces based on the player id
   ------------------------------------------------*/

  public void SetUIGamePieces()
  {
    switch (_draggerType)
    {
      case DraggerType.Tadpole:
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientTadpoleType);
        _sprite = _resourceManager.GetSprite(_gamePieceManager.ClientTadpoleType);
        break;
      case DraggerType.Frog:
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientFrogType);
        _sprite = _resourceManager.GetSprite(_gamePieceManager.ClientFrogType);
        break;
    }

    _spriteRenderer.sprite = _sprite;
  }

  /*--------------------------------------------------------------
   * This method is called when the game piece is tapped
   * Instantiate a new game piece that can be dragged on the game board
   * That game piece is then snapped to the nearest game tile
   ---------------------------------------------------------------*/
  private void DragPiece()
  {
    if (!_networkManager.IsPlayerTurn()) return;
    if (DetectTouch() && !IsDragging)
    {
      GameObject newGamePiece = Instantiate(_prefab, transform.position, Quaternion.identity);
      GameObject.Find("GameboardManager").GetComponent<GameboardManager>().LastTouchedGameTile = null;
      newGamePiece.AddComponent<DragHandler>();
      newGamePiece.GetComponent<DragHandler>().UIGamePiece = this;

      //Set the game piece type
      newGamePiece.GetComponent<DragHandler>().TypeOfPiece = _draggerType == DraggerType.Tadpole ? _gamePieceManager.ClientTadpoleType : _gamePieceManager.ClientFrogType;
      IsDragging = true;
    }
  }

  /*--------------------------------------------------------------------------
   * Detect if the player has touched the game piece
   * @return - True if the player has touched the game piece, false otherwise
   ---------------------------------------------------------------------------*/
  private bool DetectTouch()
  {
    if (Input.touchCount > 0)
    {
      //Get the touch position
      Touch touch = Input.GetTouch(0);
      Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
      touchPosition.z = 0;

      //Check if the touch position is over the game piece
      if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
      {
        return true;
      }
    }
    return false;
  }
}