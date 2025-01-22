using UnityEngine;



public class DraggableUIGamePiece : MonoBehaviour
{
  public bool IsDragging { get; set; }

  private enum DraggerType { Cat, Kitten }

  [SerializeField] private DraggerType _draggerType;

  private SpriteRenderer _spriteRenderer;
  private ResourceManager _resourceManager;
  private GamePieceManager _gamePieceManager;
  private GameObject _prefab;
  private Sprite _sprite;

  void Start()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;

    NetworkManager.Instance.OnGameInitialized += SetUIGamePieces;
  }

  void Update()
  {
    DragPiece();
  }

  private void SetUIGamePieces()
  {
    switch (_draggerType)
    {
      case DraggerType.Kitten:
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientKittenType);
        _sprite = _resourceManager.GetSprite(_gamePieceManager.ClientKittenType);
        break;
      case DraggerType.Cat:
        _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientCatType);
        _sprite = _resourceManager.GetSprite(_gamePieceManager.ClientCatType);
        break;
    }

    _spriteRenderer.sprite = _sprite;
  }

  private void DragPiece()
  {
    if (!NetworkManager.Instance.IsPlayerTurn()) return;
    if (DetectTouch() && !IsDragging)
    {
      GameObject newGamePiece = Instantiate(_prefab, transform.position, Quaternion.identity);
      GameObject.Find("GameboardManager").GetComponent<GameboardManager>().LastTouchedGameTile = null;
      newGamePiece.AddComponent<DragHandler>();
      newGamePiece.GetComponent<DragHandler>().UIGamePiece = this;

      //Set the game piece type
      newGamePiece.GetComponent<DragHandler>().TypeOfPiece = _draggerType == DraggerType.Cat ? _gamePieceManager.ClientCatType : _gamePieceManager.ClientKittenType;
      IsDragging = true;
    }
  }

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
