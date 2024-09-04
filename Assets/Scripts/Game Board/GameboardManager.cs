using UnityEngine;

public class GameboardManager : MonoBehaviour
{
  public static GameboardManager Instance { get; private set; } //Singleton instance

  //Properties
  public GameTile SelectedGameTile { get; set; } // The currently selected game tile
  public GameTile LastTouchedGameTile { get; set; } // The last touched game tile
  public bool CurrentlyTouchingGameBoard { get; private set; } // True if the player is currently touching the game board
  public GameTile[,] GameTiles { get; private set; } //2D array of GameTile objects

  [Header("Prefabs")]
  [SerializeField] private GameObject _gameTilePrefab; //Reference to the GameTile prefab

  //Private variables
  private NetworkManager _networkManager; //Reference to the NetworkManager
  private ResourceManager _resourceManager; //Reference to the ResourceManager
  private GamePieceManager _gamePieceManager; //Reference to the GamePieceManager

  //Awake is called when the script instance is being loaded
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      CreateGameboard();
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    _networkManager = NetworkManager.Instance;
    _resourceManager = ResourceManager.Instance;
    _gamePieceManager = GamePieceManager.Instance;
    _networkManager.OnTadpolePlaced += PlaceTadpole;
  }

  private void Update()
  {
    IsPlayerTouchingGameBoard();
  }

  private void PlaceTadpole(GamePieceState state)
  {

    if (state.playerId == _networkManager.PlayerId) return;

    GameObject prefab = _resourceManager.GetPrefab(_gamePieceManager.OpponentTadpoleType);

    Vector2 position = new Vector2(state.position.x, state.position.y);

    Instantiate(prefab, position, Quaternion.identity);
  }

  /*---------------------------------------------------------
  * Create a game board with the specified width and height
  * @param width - The width of the game board
  * @param height - The height of the game board
  * @param tileSize - The size of each tile in the game board
  ----------------------------------------------------------*/
  private void CreateGameboard(int width = 6, int height = 6, float tileSize = 0.65f)
  {
    GameTiles = new GameTile[width, height];

    float boardWidth = width * tileSize;
    float boardHeight = height * tileSize;

    float startX = -boardWidth / 2 + tileSize / 2;
    float startY = -boardHeight / 2 + tileSize / 2;

    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        GameObject newTile = Instantiate(_gameTilePrefab, new Vector3(startX + x * tileSize, startY + y * tileSize, 0), Quaternion.identity);
        GameTile gameTile = newTile.GetComponent<GameTile>();

        GameTiles[x, y] = gameTile;
      }
    }
  }

  /*------------------------------------------------------------------------------------------
  * Check if the player is currently touching the game board
  * Set property CurrentlyTouchingGameBoard to true if the player is touching the game board
  -------------------------------------------------------------------------------------------*/
  private void IsPlayerTouchingGameBoard()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
      touchPosition.z = 0;

      if (Physics2D.OverlapPoint(touchPosition) != null)
      {
        CurrentlyTouchingGameBoard = true;
      }
      else
      {
        CurrentlyTouchingGameBoard = false;
      }
    }
  }
}