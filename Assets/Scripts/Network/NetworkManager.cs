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
  public event Action<GameState> OnRoomCreated; //Event that is triggered when the room is created
  public event Action<BoardState> OnBoardCreated; //Event that is triggered when the board state changes
  public event Action<BoardState> OnBoardChanged; //Event that is triggered when the board state changes
  public event Action<GamePieceState> OnTadpolePlaced; //Event that is triggered when the game state changes
  public event Action<GamePieceState> OnFrogPlaced; //Event that is triggered when the game state changes
  public event Action<HandState> OnUIChanged; //Event that is triggered when the UI state changes

  /* Private variables */
  private bool _boardCreated = false;
  private ColyseusClient _client; //Reference to the Colyseus client
  private ColyseusRoom<GameState> _room; //Reference to the Game room
  private UIManager _uiManager; //Reference to the UIManager
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
    _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    _gameboardManager = GameboardManager.Instance;

    //Test
  }

  /*------------------------------------------
  * Create a new room on the server
  * @param roomName - The name of the room
  * @return Task - The task to create a room
  -------------------------------------------*/
  public async Task CreateRoom(string roomName)
  {
    _room = await _client.Create<GameState>(roomName); //Create a new room on the server
    RegisterRoomHandlers(); //Register the room handlers
    await _room.Send("createRoom");

    GetClientId();
    GetRoomId();

    _uiManager.SetRoomCode(RoomId);

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

    _uiManager.DisableRoomCodeText();

    PlayerId = 2;
    InitializeUIListener();
    GamePieceManager.Instance.SetFrogType(PlayerId);
    GamePieceManager.Instance.SetTadpoleType(PlayerId);
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
    NullCheckRoom();

    /*-------------------------------------------
    * Trigger events when the game state changes
    ---------------------------------------------*/
    _room.State.board.OnChange(() =>
    {
      if (_room.State.board == null) return;

      if (!_boardCreated)
      {
        _boardCreated = true;
        OnBoardCreated?.Invoke(_room.State.board);
      }
    });

    /*--------------------------
    * Messages from the server
    ----------------------------*/
    _room.OnMessage<string>("choosePieceToEvolve", (_msg) =>
    {
      _gameboardManager.SelectTadpoleToEvolve();
    });

  }

  public void SendEvolvingTadpole(int x, int y)
  {
    if (NullCheckRoom()) return;
    _room.Send("evolveTadpole", new { x, y, playerId = PlayerId });
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
        OnBoardChanged?.Invoke(_room.State.board);
      });
    });
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
    if (PlayerId == 1)
    {
      _room.State.playerOne.hand.OnChange(() =>
      {
        OnUIChanged?.Invoke(_room.State.playerOne.hand);
      });
    }
    else if (PlayerId == 2)
    {
      _room.State.playerTwo.hand.OnChange(() =>
      {
        OnUIChanged?.Invoke(_room.State.playerTwo.hand);
      });
    }
    else
    {
      Debug.LogError("Player id is not set");
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
    if (_room == null)
    {
      Debug.LogError("Room is null");
      return true;
    }
    return false;
  }
}