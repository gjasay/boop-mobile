using UnityEngine;

public class DraggableUIGamePiece : MonoBehaviour
{
  public bool IsDragging { get; set; } //True if the game piece is being dragged
  [SerializeField] private GamePieceType gamePieceType; //The type of game piece
  private SpriteRenderer _spriteRenderer; //Reference to the sprite renderer
  private ResourceManager _resourceManager; //Reference to the resource manager
  private GameObject _prefab; //Reference to the prefab to be instantiated

  // Start is called before the first frame update
  void Start()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _resourceManager = ResourceManager.Instance;

    _spriteRenderer.sprite = _resourceManager.GetSprite(gamePieceType);
    _prefab = _resourceManager.GetPrefab(gamePieceType);
  }

  // Update is called once per frame
  void Update()
  {
    DragPiece();
  }

  /*--------------------------------------------------------------
   * This method is called when the game piece is tapped
   * Instantiate a new game piece that can be dragged on the game board
   * That game piece is then snapped to the nearest game tile
   ---------------------------------------------------------------*/
  private void DragPiece()
  {
    if (DetectTouch() && !IsDragging)
    {
      GameObject newGamePiece = Instantiate(_prefab, transform.position, Quaternion.identity);
      GameObject.Find("GameboardManager").GetComponent<GameboardManager>().LastTouchedGameTile = null;
      newGamePiece.AddComponent<PieceDragging>();
      newGamePiece.GetComponent<PieceDragging>().UIGamePiece = this;
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