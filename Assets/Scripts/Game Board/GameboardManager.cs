using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameboardManager : MonoBehaviour
{
  public static GameboardManager Instance { get; private set; } //Singleton instance

  //Properties
  public GameTile LastTouchedGameTile { get; set; } // The last touched game tile
  public bool CurrentlyTouchingGameBoard { get; private set; } // True if the player is currently touching the game board
  public GameTile[,] GameTiles { get; private set; } //2D array of GameTile objects

  [FormerlySerializedAs("_gameTilePrefab")]
  [Header("Prefabs")]
  [SerializeField] private GameObject gameTilePrefab; //Reference to the GameTile prefab

  //Private variables
  private NetworkManager _networkManager; //Reference to the NetworkManager
  private ResourceManager _resourceManager; //Reference to the ResourceManager
  private GamePieceManager _gamePieceManager; //Reference to the GamePieceManager
  private TouchDetection _touch;

  //Awake is called when the script instance is being loaded
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  //Start is called before the first frame update
  private void Start()
  {
    //Get references to the managers
    _networkManager = NetworkManager.Instance;
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;
    _touch = TouchDetection.Instance;

    //Subscribe to events
    _networkManager.OnBoardCreated += CreateGameboard;
    _networkManager.OnTileChange += HandlePlacement;
  }

  private void Update()
  {
    IsPlayerTouchingGameBoard();
  }

  /*---------------------------------------------------------
  * Network event handlers
  ----------------------------------------------------------*/
  public void SelectTadpoleToEvolve()
  {
    Debug.Log("Choose piece to evolve");
    LastTouchedGameTile = null;

    StartCoroutine(SendEvolvingTadpole());
  }

  private IEnumerator SendEvolvingTadpole()
  {
    yield return new WaitUntil(() => LastTouchedGameTile != null);

    if (LastTouchedGameTile.CurrentlyHeldPiece.pieceType == "tadpole")
    {
      Debug.Log(LastTouchedGameTile.ArrayPosition.x + " " + LastTouchedGameTile.ArrayPosition.y);
      _networkManager.SendEvolvingTadpole(LastTouchedGameTile.ArrayPosition.x, LastTouchedGameTile.ArrayPosition.y);
    }
    else
    {
      Debug.Log("Not a tadpole");
      SelectTadpoleToEvolve();
    }
  }

  /*---------------------------------------------------------
  * Place a tadpole on the game board
  * @param state - The state of the game piece to place
  ----------------------------------------------------------*/
  private void PlaceTadpole(int tileX, int tileY, int playerId)
  {
    GamePieceType type = playerId == _networkManager.PlayerId ? _gamePieceManager.ClientTadpoleType : _gamePieceManager.OpponentTadpoleType;

    GameObject prefab = _resourceManager.GetPrefab(type);

    GameTile gameTile = GameTiles[tileX, tileY];

    GameObject tadpole = Instantiate(prefab, gameTile.transform.position, Quaternion.identity);
    PlacedPiece gamePiece = tadpole.AddComponent<PlacedPiece>();
    gamePiece.SetTilePlacement(gameTile);
    gamePiece.SetPieceType("tadpole");
  }

  /*---------------------------------------------------------
  * Place a frog on the game board
  * @param state - The state of the game piece to place
  ----------------------------------------------------------*/
  private void PlaceFrog(int tileX, int tileY, int playerId)
  {
    GamePieceType type = playerId == _networkManager.PlayerId ? _gamePieceManager.ClientFrogType : _gamePieceManager.OpponentFrogType;

    GameObject prefab = _resourceManager.GetPrefab(type);

    GameTile gameTile = GameTiles[tileX, tileY];

    GameObject frog = Instantiate(prefab, gameTile.transform.position, Quaternion.identity);
    PlacedPiece gamePiece = frog.AddComponent<PlacedPiece>();
    gamePiece.SetTilePlacement(gameTile);
    gamePiece.SetPieceType("frog");
  }

  private void PlacePiece(int tileX, int tileY, int playerId, string pieceType)
  {
    switch (pieceType)
    {
      case "tadpole":
        PlaceTadpole(tileX, tileY, playerId);
        break;
      case "frog":
        PlaceFrog(tileX, tileY, playerId);
        break;
    }
  }

  /*---------------------------------------------------------
  * Create the game board from the server
  ----------------------------------------------------------*/
  private void CreateGameboard(BoardState state)
  {
    Debug.Log("Creating game board");
    const float tileSize = 0.67f;
    GameTiles = new GameTile[state.width, state.height];

    float boardWidth = state.width * tileSize;
    float boardHeight = state.height * tileSize;

    float startX = -boardWidth / 2 + tileSize / 2;
    float startY = -boardHeight / 2 + tileSize / 2;

    state.tiles.ForEach((tile) =>
    {
      ArrayCoordinate coordinate = tile.arrayPosition;
      GameTile gameTile = Instantiate(gameTilePrefab, new Vector3(startX + coordinate.x * tileSize, startY + coordinate.y * tileSize, 0), Quaternion.identity).GetComponent<GameTile>();
      gameTile.ArrayPosition = new Vector2Int(coordinate.x, coordinate.y);

      GameTiles[coordinate.x, coordinate.y] = gameTile;
    });

    _networkManager.InitializeTileListener();
  }

  private void HandlePlacement(TileState state)
  {
    GameTile gameTile = GameTiles[state.arrayPosition.x, state.arrayPosition.y];

    if (state.gamePiece is { type: not null, priorCoordinate: not null } && state.gamePiece.priorCoordinate.x != -1 && state.gamePiece.priorCoordinate.y != -1)
    {
      GameTile priorGameTile = GameTiles[state.gamePiece.priorCoordinate.x, state.gamePiece.priorCoordinate.y];
      Debug.Log("Moving piece");
      StartCoroutine(MovePieceToTile(priorGameTile, gameTile, 0.5f));
    } 
    else if (state.gamePiece != null && gameTile.CurrentlyHeldPiece == null)
    {
      PlacePiece(state.arrayPosition.x, state.arrayPosition.y, state.gamePiece.playerId, state.gamePiece.type);
    }
    else if (state.gamePiece == null && !string.IsNullOrEmpty(state.outOfBounds))
    {
      Debug.Log("Out of bounds direction: " + state.outOfBounds);
      StartCoroutine(MovePieceOutOfBounds(gameTile, state.outOfBounds, 0.5f));
    }
    else if (state.gamePiece == null && gameTile.CurrentlyHeldPiece != null && gameTile.CurrentlyHeldPiece.IsMoving == false)
    {
      Destroy(gameTile.CurrentlyHeldPiece.gameObject);
    }
  }

  IEnumerator MovePieceToTile(GameTile origin, GameTile destination, float duration)
  {
    if (!origin.CurrentlyHeldPiece) yield break;
    origin.CurrentlyHeldPiece.IsMoving = true;

    float time = 0;
    Vector3 startPosition = origin.CurrentlyHeldPiece.transform.position;
    Vector3 endPosition = destination.transform.position;

    while (time < duration)
    {
      origin.CurrentlyHeldPiece.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
      time += Time.deltaTime;
      yield return null;
    }

    origin.CurrentlyHeldPiece.transform.position = endPosition;

    if (origin.CurrentlyHeldPiece)
    {
      origin.CurrentlyHeldPiece.SetTilePlacement(destination);
      origin.CurrentlyHeldPiece = null;

      destination.CurrentlyHeldPiece.IsMoving = false;
    }

    _networkManager.SendPieceMoved(destination);
  }

  IEnumerator MovePieceOutOfBounds(GameTile origin, string direction, float duration)
  {
    if (origin.CurrentlyHeldPiece == null) yield break;
    origin.CurrentlyHeldPiece.IsMoving = true;

    float time = 0;
    float offset = 0.65f;
    Vector3 startPosition = origin.CurrentlyHeldPiece.transform.position;
    Vector3 endPosition;

    switch (direction)
    {
      case "top":
        endPosition = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);
        break;
      case "bottom":
        endPosition = new Vector3(startPosition.x, startPosition.y - offset, startPosition.z);
        break;
      case "left":
        endPosition = new Vector3(startPosition.x - offset, startPosition.y, startPosition.z);
        break;
      case "right":
        endPosition = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
        break;
      case "top-left":
        endPosition = new Vector3(startPosition.x - offset, startPosition.y + offset, startPosition.z);
        break;
      case "top-right":
        endPosition = new Vector3(startPosition.x + offset, startPosition.y + offset, startPosition.z);
        break;
      case "bottom-left":
        endPosition = new Vector3(startPosition.x - offset, startPosition.y - offset, startPosition.z);
        break;
      case "bottom-right":
        endPosition = new Vector3(startPosition.x + offset, startPosition.y - offset, startPosition.z);
        break;
      default:
        Debug.LogError("Invalid direction");
        endPosition = startPosition;
        break;
    }

    while (time < duration)
    {
      origin.CurrentlyHeldPiece.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
      time += Time.deltaTime;
      yield return null;
    }

    origin.CurrentlyHeldPiece.transform.position = endPosition;

    if (origin.CurrentlyHeldPiece != null)
    {
      Destroy(origin.CurrentlyHeldPiece.gameObject);
    }

    origin.CurrentlyHeldPiece = null;

    _networkManager.SendPieceMoved(origin);
  }



  /*------------------------------------------------------------------------------------------
  * Check if the player is currently touching the game board
  * Set property CurrentlyTouchingGameBoard to true if the player is touching the game board
  -------------------------------------------------------------------------------------------*/
  private void IsPlayerTouchingGameBoard()
  {
    if (Input.touchCount <= 0) return;
    Touch touch = Input.GetTouch(0);
    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    touchPosition.z = 0;

    CurrentlyTouchingGameBoard = Physics2D.OverlapPoint(touchPosition) != null;
  }
}