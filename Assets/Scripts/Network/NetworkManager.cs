using UnityEngine;
using Colyseus;
using System;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
  public static NetworkManager Instance { get; private set; } //Singleton instance

  /* Properties */
  public int PlayerId { get; private set; } // The player id of the player
  public string ClientId { get; private set; } // The client id of the player
  public string RoomId { get; private set; } // The room id of the room

  /* Events */
  public event Action<BoardState> OnBoardCreated; //Event that is triggered when the board state changes
  public event Action OnPlayerJoined;
  public event Action<TileState> OnTileChange; //Event that is triggered when the board state changes
  public event Action<HandState> OnHandChanged; //Event that is triggered when the UI state changes
  public event Action<PlayerState> OnClientUpdate; //Event that is triggered when the client player state changes
  public event Action<PlayerState> OnOpponentUpdate; //Event that is triggered when the opponent player state changes
  public event Action<int> OnPlayerWin; //Event that is triggered when a player wins

  /* Private variables */
  private bool _boardCreated = false;
  private ColyseusClient _client; //Reference to the Colyseus client
  private ColyseusRoom<GameState> _room; //Reference to the Game room
  private MainUIEventHandler _uiManager; //Reference to the UIManager
  private GameboardManager _gameboardManager; //Reference to the GameboardManager

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      InitializeClient();
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();
    _gameboardManager = GameboardManager.Instance;
  }

  /*------------------------------------------
  * Create a new room on the server
  * @param roomName - The name of the room
  * @return Task - The task to create a room
  -------------------------------------------*/
  public async Task CreateRoom(string roomName, int time)
  {
    _room = await _client.Create<GameState>(roomName); //Create a new room on the server
    RegisterRoomHandlers(); //Register the room handlers
    await _room.Send("createRoom", new { time });

    GetClientId();
    GetRoomId();

    PlayerId = 1;
    InitializeUIListener();
    GamePieceManager.Instance.SetFrogType(PlayerId);
    GamePieceManager.Instance.SetTadpoleType(PlayerId);
  }

  /*--------------------------------------------
  * Join an existing room on the server
  * @param roomId - The id of the room to join
  * @return Task - The task to join a room
  ----------------------------------------------*/
  public async Task JoinRoom(string roomId)
  {
    _room = await _client.JoinOrCreate<GameState>("my_room"); //Join a colyseus room
    RegisterRoomHandlers(); //Register the room handlers

    GetClientId();
    GetRoomId();

    PlayerId = 2;
    InitializeUIListener();
    GamePieceManager.Instance.SetFrogType(PlayerId);
    GamePieceManager.Instance.SetTadpoleType(PlayerId);
  }

  public async Task JoinOrCreateRoom()
  {
    _room = await _client.JoinOrCreate<GameState>("my_room"); //Join a colyseus room
    RegisterRoomHandlers(); //Register the room handlers

    GetClientId();
    GetRoomId();

    // _uiManager.DisableRoomCodeText();

    //check if player created or joined room
    // if (_room.State.plq)

    PlayerId = 2;
    InitializeUIListener();
    GamePieceManager.Instance.SetFrogType(PlayerId);
    GamePieceManager.Instance.SetTadpoleType(PlayerId);
  }
  
  public void LeaveRoom()
  {
    if (NullCheckRoom()) return;
    _room.Leave();
  }
  
  public void ReconnectToRoom()
  {
    if (NullCheckRoom()) return;
    // _room.Connect();
    Debug.Log("Reconnecting to room");
  }

  /*-------------------------------
  * Initialize the Colyseus client
  ---------------------------------*/
  private void InitializeClient()
  {
    _client = new ColyseusClient("ws://localhost:2567"); //Create a new Colyseus client
  }

  /*---------------------------
  * Register the room handlers
  -----------------------------*/
  private void RegisterRoomHandlers()
  {
    if (NullCheckRoom()) return;

    /*-------------------------------------------
    * Trigger events when the game state changes
    ---------------------------------------------*/
    _room.State.board.OnChange(() =>
    {
      if (_room.State.board == null) return;

      if (_boardCreated) return;
      _boardCreated = true;
      OnBoardCreated?.Invoke(_room.State.board);
    });

    _room.State.OnWinnerChange((value, prev) => { OnPlayerWin?.Invoke(value); });

    _room.State.playerOne.OnChange(() =>
    {
      if (PlayerId == 1)
        OnClientUpdate?.Invoke(_room.State.playerOne);
      else
        OnOpponentUpdate?.Invoke(_room.State.playerOne);
    });
    
    _room.State.playerTwo.OnChange(() =>
    {
      if (PlayerId == 2)
        OnClientUpdate?.Invoke(_room.State.playerTwo);
      else
        OnOpponentUpdate?.Invoke(_room.State.playerTwo);
    });
    
    /*--------------------------
    * Messages from the server
    ----------------------------*/
    _room.OnMessage<string>("choosePieceToEvolve", (msg) => { _gameboardManager.SelectTadpoleToEvolve(); });

    _room.OnMessage<string>("playerJoined", (msg) => { OnPlayerJoined?.Invoke(); });
  }

  public void SendEvolvingTadpole(int x, int y)
  {
    if (NullCheckRoom()) return;
    _room.Send("evolveTadpole", new { x, y, playerId = PlayerId });
  }

  public void SendPieceMoved(GameTile tile)
  {
    if (NullCheckRoom()) return;
    _room.Send("pieceMoved", new { x = tile.ArrayPosition.x, y = tile.ArrayPosition.y });
  }

  /*-------------------------------------------
  * Initialize the tile listener
  ---------------------------------------------*/
  public void InitializeTileListener()
  {
    _room.State.board.tiles.ForEach(tile =>
    {
      tile.OnChange(() =>
      {
        OnTileChange?.Invoke(
          _room.State.board.tiles[
            tile.arrayPosition.y * _room.State.board.width + tile.arrayPosition.x]);
      });
    });
  }

  public bool IsPlayerTurn()
  {
    if (NullCheckRoom()) return false;
    return _room.State.currentPlayer == PlayerId;
  }

  /*-------------------------------------------
  * Request to place a tadpole on the board
  * @param x - The x position of the piece
  * @param y - The y position of the piece
  * @param type - The type of the piece (tadpole or frog)
  ---------------------------------------------*/
  public void PlacePiece(int x, int y, string type)
  {
    if (NullCheckRoom()) return;
    _room.Send("placePiece", new { x, y, type, playerId = PlayerId });
  }

  private void InitializeUIListener()
  {
    Debug.Log("Initializing UI Listener");
    if (NullCheckRoom()) return;
    Debug.Log("Room is not null");
    switch (PlayerId)
    {
      case 1:
        _room.State.playerOne.hand.OnChange(() => { OnHandChanged?.Invoke(_room.State.playerOne.hand); });
        break;
      case 2:
        _room.State.playerTwo.hand.OnChange(() => { OnHandChanged?.Invoke(_room.State.playerTwo.hand); });
        break;
      default:
        Debug.LogError("Player id is not set");
        break;
    }
  }

  /*------------------
  * Get the client id
  --------------------*/
  private void GetClientId()
  {
    if (NullCheckRoom()) return;
    ClientId = _room.SessionId;
  }

  /*------------------
  * Get the room id
  --------------------*/
  private void GetRoomId()
  {
    if (NullCheckRoom()) return;
    RoomId = _room.RoomId;
  }

  /*--------------------------
  * Check if the room is null
  ----------------------------*/
  private bool NullCheckRoom()
  {
    if (_room != null) return false;
    Debug.LogError("Room is null");
    return true;
  }
}