using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainUIEventHandler : MonoBehaviour
{
  public bool IsDragging { get; set; }

  private UIDocument _document;
  private VisualElement _root;
  private Image _kitten;
  private Image _cat;
  private Label _numOfKittens;
  private Label _numOfCats;
  private Label _resultText;
  private Label _playerTimer;
  private Label _opponentTimer;
  private Box _resultBox;
  private Box _lobbyBox;
  private Button _playAgainButton;
  private Button _mainMenuButton;
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
    _networkManager.OnPlayerWin += HandlePlayerWin;
    _networkManager.OnClientUpdate += UpdatePlayerTimer;
    _networkManager.OnOpponentUpdate += UpdateOpponentTimer;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      _lobbyBox.ToggleInClassList("visible");
    }
  }

  private void OnEnable()
  {
    _document = GetComponent<UIDocument>();
    _root = _document.rootVisualElement;

    _kitten = _root.Q<Image>("tadpole-image");
    _cat = _root.Q<Image>("frog-image");

    _numOfKittens = _root.Q<Label>("tadpole-count");
    _numOfCats = _root.Q<Label>("frog-count");

    _playerTimer = _root.Q<Label>("player-timer");
    _opponentTimer = _root.Q<Label>("opponent-timer");

    _resultText = _root.Q<Label>("result");

    _resultBox = _root.Q<Box>("result-box");
    _lobbyBox = _root.Q<Box>("lobby-box");

    _playAgainButton = _root.Q<Button>("play-again-button");
    _mainMenuButton = _root.Q<Button>("main-menu-button");

    _kitten.RegisterCallback((PointerEnterEvent evt, Image root) => HandleTadpoleClick(evt, root), _kitten);
    _cat.RegisterCallback((PointerEnterEvent evt, Image root) => HandleFrogClick(evt, root), _cat);

    _mainMenuButton.clicked += () => { SceneManager.LoadScene(1); };
  }

  public void SetUIGamePieces()
  {
    _kitten.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientTadpoleType);
    _cat.sprite = _resourceManager.GetSprite(_gamePieceManager.ClientFrogType);
  }

  // check if the player has greater than 0 of the piece
  public bool CanPlacePiece(string type)
  {
    switch (type)
    {
      case "tadpole" when _numOfKittens.text == "0":
      case "frog" when _numOfCats.text == "0":
        return false;
      default:
        return true;
    }
  }

  private void UpdatePlayerTimer(PlayerState state)
  {
    TimeSpan time = TimeSpan.FromSeconds(state.timer);
    _playerTimer.text = $"{time.Minutes}:{time.Seconds:D2}";
  }

  private void UpdateOpponentTimer(PlayerState state)
  {
    TimeSpan time = TimeSpan.FromSeconds(state.timer);
    _opponentTimer.text = $"{time.Minutes}:{time.Seconds:D2}";
  }

  private void HandlePlayerWin(int playerId)
  {
    _resultText.text = playerId == _networkManager.PlayerId ? "You Win!" : "You Lose!";
    _resultBox.ToggleInClassList("visible");
  }

  private void UpdateHandCount(HandState hand)
  {
    _numOfKittens.text = hand.tadpoles.ToString();
    _numOfCats.text = hand.frogs.ToString();
  }

  private void HandleTadpoleClick(PointerEnterEvent evt, Image root)
  {
    if (!_networkManager.IsPlayerTurn() || IsDragging) return;
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