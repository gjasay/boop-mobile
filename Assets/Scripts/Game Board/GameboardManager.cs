using Colyseus;
using UnityEngine;

public class GameboardManager : MonoBehaviour
{
  //Properties
  public GameTile SelectedGameTile { get; set; } // The currently selected game tile
  public GameTile LastTouchedGameTile { get; set; } // The last touched game tile
  public bool CurrentlyTouchingGameBoard { get; private set; } // True if the player is currently touching the game board
  public int PlayerId { get; private set; } // The player id of the player (1 or 2)
  public GameTile[,] GameTiles { get; private set; } //2D array of GameTile objects

  [Header("Prefabs")]
  [SerializeField] private GameObject _gameTilePrefab; //Reference to the GameTile prefab
  [SerializeField] private GameObject _orangeTadpolePrefab; //Reference to the Orange Tadpole prefab
  [SerializeField] private GameObject _purpleTadpolePrefab; //Reference to the Purple Tadpole prefab

  //Private variables
  private UIManager _uiManager; //Reference to the UIManager
  private ColyseusClient _client; //Reference to the Colyseus client
  private ColyseusRoom<GameState> _room; //Reference to the Game room
  private string _clientId; // The client id of the player
  private string _roomId; // The room id of the room

  private void Start()
  {
    _uiManager = GameObject.Find("UI").GetComponent<UIManager>(); //Get the UIManager component

    //Testing
  }

  private void Update()
  {
    IsPlayerTouchingGameBoard();
  }

  public void SendTadpolePlacement(GamePieceState gamePieceState)
  {
    _room.Send("placeTadpole", gamePieceState);

    
  }

  /*---------------------------------------
   * Create a new room on the server
   ----------------------------------------*/
  public async void CreateRoom()
  {
    _client = new ColyseusClient("ws://localhost:2567"); //Create a new Colyseus client
    _room = await _client.Create<GameState>("my_room"); //Create a new room on the server

    RegisterRoomHandlers();

    await _room.Send("createRoom"); //Send a message to the server to create a room

    GetClientId();
    GetRoomId();

    CreateGameboard(); //Create a 6x6 game board

    PlayerId = 1;
  }

  /*---------------------------------------
   * Join an existing room on the server
   * @param roomId - The id of the room to join
   ----------------------------------------*/
  public async void JoinRoom(string roomId)
  {
    _client = new ColyseusClient("ws://localhost:2567"); //Create a new Colyseus client
    _room = await _client.JoinById<GameState>(roomId); //Join a colyseus room

    RegisterRoomHandlers();

    await _room.Send("joinRoom"); //Send a message to the server to join the room

    _uiManager.DisableRoomCodeText(); //Disable the room code text

    GetClientId();
    CreateGameboard(); //Create a 6x6 game board

    PlayerId = 2; //Set the player id to 2
  }

  private void RegisterRoomHandlers()
  {
    _room.OnMessage<GamePieceState>("tadpolePlaced", (message) =>
    {
      Debug.Log("Tadpole placed at: " + message.position.x + ", " + message.position.y);

      if (message.playerId == PlayerId) return;

      if (PlayerId == 1)
      {
        GameObject tadpole = Instantiate(_orangeTadpolePrefab, new Vector3(message.position.x, message.position.y, 0), Quaternion.identity);
      }
      else if (PlayerId == 2)
      {
        GameObject tadpole = Instantiate(_purpleTadpolePrefab, new Vector3(message.position.x, message.position.y, 0), Quaternion.identity);
      }
    });
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

  /*---------------------------------------
   * Get the client id of the player
   ----------------------------------------*/
  private void GetClientId()
  {
    _room.OnMessage<string>("sessionId", (message) =>
    {
      _clientId = message;
      Debug.Log("Client ID: " + _clientId);
    });
  }

  /*---------------------------------------
   * Get the room id of the room
  ----------------------------------------*/
  private void GetRoomId()
  {
    _room.OnMessage<string>("roomId", (message) =>
    {
      _roomId = message;

      if (_uiManager == null) return;
      _uiManager.SetRoomCode(_roomId);
    });
  }

  /*------------------------------------------------
   * Check if the player is touching the game board
   -------------------------------------------------*/
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