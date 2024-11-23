using UnityEngine;
using UnityEngine.UIElements;

public class MainUIEventHandler : MonoBehaviour
{
  public bool IsDragging { get; set; }
    
  private UIDocument _document;
  private VisualElement _root;
  private Image _tadpole;
  private Image _frog;
  private Label _numOfTadpoles;
  private Label _numOfFrogs;
  private Label _info;
  private ResourceManager _resourceManager;
  private GamePieceManager _gamePieceManager;
  private NetworkManager _networkManager;
  private GameObject _prefab;
  private TouchDetection _touch;

  private void Start()
  {
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;
    _networkManager = NetworkManager.Instance;
    _touch = TouchDetection.Instance;

    _networkManager.OnHandChanged += UpdateHandCount;
    _networkManager.OnPlayerJoined += ClearInfoLabel;
  }

  private void OnEnable()
  {
    _document = GetComponent<UIDocument>();
    _root = _document.rootVisualElement;

    _tadpole = _root.Q<Image>("tadpole-image");
    _frog = _root.Q<Image>("frog-image");
    _numOfTadpoles = _root.Q<Label>("tadpole-count");
    _numOfFrogs = _root.Q<Label>("frog-count");
    _info = _root.Q<Label>("info");

    //Will using click events cause issues for mobile??
    _tadpole.RegisterCallback((PointerEnterEvent evt, Image root) => HandleTadpoleClick(evt, root), _tadpole);
    _frog.RegisterCallback((PointerEnterEvent evt, Image root) => HandleFrogClick(evt, root), _frog);
  }

  public void SetRoomCode(string code)
  {
    _info.text += $"\nRoom Code: {code}";
  }
    
  public void SetInfoLabel(string info)
  {
    _info.text = info;
  }

  public void SetUIGamePieces()
  {
    _tadpole.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientTadpoleType);
    _frog.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientFrogType);
  }

  // check if the player has greater than 0 of the piece
  public bool CanPlacePiece(string type)
  {
    switch (type)
    {
      case "tadpole" when _numOfTadpoles.text == "0":
      case "frog" when _numOfFrogs.text == "0":
        return false;
      default:
        return true;
    }
  }

  public void ClearInfoLabel()
  {
    _info.text = ""; 
  }

  private void UpdateHandCount(HandState hand)
  {
    _numOfTadpoles.text = hand.tadpoles.ToString();
    _numOfFrogs.text = hand.frogs.ToString();
  }
  
  private void HandleTadpoleClick(PointerEnterEvent evt, Image root)
  {
    Debug.Log("Tadpole click");
    if (!_networkManager.IsPlayerTurn() || IsDragging) return;
    Debug.Log("Tadpole clicked");
    _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientTadpoleType);
    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    GameObject gamePiece = Instantiate(_prefab, position, Quaternion.identity);
    gamePiece.AddComponent<DragHandler>();

    gamePiece.GetComponent<DragHandler>().TypeOfPiece = _gamePieceManager.ClientTadpoleType;
    gamePiece.GetComponent<DragHandler>().PieceType = "tadpole";
    IsDragging = true;
    root.sprite = null;
  }

  private void HandleFrogClick(PointerEnterEvent evt, Image root)
  {
    if (!_networkManager.IsPlayerTurn() || IsDragging) return;
    Debug.Log("Frog clicked");
    _prefab = _resourceManager.GetPrefab(_gamePieceManager.ClientFrogType);
    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    GameObject gamePiece = Instantiate(_prefab, position, Quaternion.identity);
    gamePiece.AddComponent<DragHandler>();
        
    gamePiece.GetComponent<DragHandler>().TypeOfPiece = _gamePieceManager.ClientFrogType;
    gamePiece.GetComponent<DragHandler>().PieceType = "frog";
    IsDragging = true;
    root.sprite = null;
  }
}