using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainUIEventHandler : MonoBehaviour
{
  public bool IsDragging { get; set; }
  public DraggableUIGamePiece Kitten { get; private set; }
  public DraggableUIGamePiece Cat { get; private set; }

  [SerializeField]
  private UnityEngine.UI.Text _numOfKittens;
  [SerializeField]
  private UnityEngine.UI.Text _numOfCats;

  private UIDocument _document;
  private VisualElement _root;
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
    Kitten = GameObject.Find("UIKitten").GetComponent<DraggableUIGamePiece>();
    Cat = GameObject.Find("UICat").GetComponent<DraggableUIGamePiece>();

    _networkManager.OnHandChanged += UpdateHandCount;
    _networkManager.OnPlayerWin += HandlePlayerWin;
    _networkManager.OnClientUpdate += UpdatePlayerTimer;
    _networkManager.OnOpponentUpdate += UpdateOpponentTimer;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space)) {
      _lobbyBox.ToggleInClassList("visible");
    }
    if (Input.GetKeyDown(KeyCode.R)) {
      Destroy(GameboardManager.Instance.gameObject);
      NetworkManager.Instance.LeaveRoom();
      // Destroy(NetworkManager.Instance.gameObject);
      Destroy(ResourceManager.Instance.gameObject);
      Destroy(GamePieceManager.Instance.gameObject);
      SceneManager.LoadScene(1);
    }
  }

  private void OnEnable()
  {
    _document = GetComponent<UIDocument>();
    _root = _document.rootVisualElement;

    _playerTimer = _root.Q<Label>("player-timer");
    _opponentTimer = _root.Q<Label>("opponent-timer");

    _resultText = _root.Q<Label>("result");

    _resultBox = _root.Q<Box>("result-box");
    _lobbyBox = _root.Q<Box>("lobby-box");

    _playAgainButton = _root.Q<Button>("play-again-button");
    _mainMenuButton = _root.Q<Button>("main-menu-button");

    _mainMenuButton.clicked += () => { 
      DestroyImmediate(GameboardManager.Instance.gameObject);
      // DestroyImmediate(NetworkManager.Instance.gameObject);
      DestroyImmediate(ResourceManager.Instance.gameObject);
      DestroyImmediate(GamePieceManager.Instance.gameObject);
      SceneManager.LoadScene(1); };
  }

  // check if the player has greater than 0 of the piece
  public bool CanPlacePiece(string type)
  {
    switch (type) {
      case "kitten" when _numOfKittens.text == "0":
      case "cat" when _numOfCats.text == "0":
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
    Debug.Log("Updating hand count");
    _numOfKittens.text = hand.kittens.ToString();
    _numOfCats.text = hand.cats.ToString();
  }
}
