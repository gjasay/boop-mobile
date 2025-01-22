using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardManager : MonoBehaviour
{
  public static GameboardManager Instance { get; private set; }

  public GameTile LastTouchedGameTile { get; set; }
  public bool CurrentlyTouchingGameBoard { get; private set; }
  public GameTile[,] GameTiles { get; private set; }
  public Dictionary<int, GamePiece> GamePieces = new Dictionary<int, GamePiece>();

  [Header("Prefabs")]
  [SerializeField]
  private GameObject gameTilePrefab;

  private NetworkManager _networkManager;
  private ResourceManager _resourceManager;
  private GamePieceManager _gamePieceManager;
  private MainUIEventHandler _uiManager;
  private TouchDetection _touch;

  void Awake()
  {
    if (Instance == null) {
      Instance = this;
      GamePieces = new Dictionary<int, GamePiece>();
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    _networkManager = NetworkManager.Instance;
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();
    _touch = TouchDetection.Instance;

    _networkManager.OnBoardCreated += CreateGameboard;
  }

  void Update()
  {
    IsPlayerTouchingGameBoard();
  }

  public void SelectTadpoleToEvolve()
  {
    Debug.Log("Choose piece to evolve");
    LastTouchedGameTile = null;

    StartCoroutine(SendEvolvingTadpole());
  }

  void OnDestroy()
  {
    GamePieces = new Dictionary<int, GamePiece>();
  }

  private IEnumerator SendEvolvingTadpole()
  {
    yield return new WaitUntil(() => LastTouchedGameTile != null);

    if (LastTouchedGameTile.CurrentlyHeldPiece.pieceType == "kitten") {
      Debug.Log(LastTouchedGameTile.ArrayPosition.x + " " + LastTouchedGameTile.ArrayPosition.y);
      _networkManager.SendEvolvingTadpole(LastTouchedGameTile.ArrayPosition.x,
                                          LastTouchedGameTile.ArrayPosition.y);
    } else {
      Debug.Log("Not a tadpole");
      SelectTadpoleToEvolve();
    }
  }

  private void CreateGameboard(GameState state)
  {
    Debug.Log("Creating game board");
    const float tileSize = 0.67f;
    GameTiles = new GameTile[state.boardSize.x, state.boardSize.y];

    float boardWidth = state.boardSize.x * tileSize;
    float boardHeight = state.boardSize.y * tileSize;

    float startX = -boardWidth / 2 + tileSize / 2;
    float startY = -boardHeight / 2 + tileSize / 2;

    state.tiles.ForEach((tile) =>
                        {
                          Vector2Schema coordinate = tile.coordinate;
                          GameTile gameTile =
                              Instantiate(gameTilePrefab,
                                          new Vector3(startX + coordinate.x * tileSize,
                                                      startY + coordinate.y * tileSize, 0),
                                          Quaternion.identity)
                                  .GetComponent<GameTile>();
                          gameTile.ArrayPosition = new Vector2Int(coordinate.x, coordinate.y);
                          GameTiles[coordinate.x, coordinate.y] = gameTile;
                        });
  }

  public void IsPlayerTouchingGameBoard()
  {
    if (Input.touchCount <= 0)
      return;
    Touch touch = Input.GetTouch(0);
    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    touchPosition.z = 0;

    CurrentlyTouchingGameBoard = Physics2D.OverlapPoint(touchPosition) != null;
  }
}
