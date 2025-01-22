using System;
using System.Threading.Tasks;
using Colyseus;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
  public static NetworkManager Instance { get; private set; }  // Singleton instance

  public int PlayerId { get; private set; }
  public string ClientId { get; private set; }
  public string RoomId { get; private set; }

  public event Action OnGameInitialized;
  public event Action<GameState> OnBoardCreated;
  public event Action OnPlayerJoined;
  public event Action<Vector2Int> OnTileChange;
  public event Action<HandState> OnHandChanged;
  public event Action<PlayerState> OnClientUpdate;
  public event Action<PlayerState> OnOpponentUpdate;
  public event Action<int> OnPlayerWin;

  private bool _boardCreated = false;
  private bool _roomHandlersRegistered = false;
  private ColyseusClient _client;
  private ColyseusRoom<GameState> _room;
  private MainUIEventHandler _uiManager;
  private GameboardManager _gameboardManager;

  private void Awake()
  {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
    } else {
      Instance = this;
      Debug.Log("Good morning!");
      InitializeClient();
      // DontDestroyOnLoad(gameObject);
    }
  }

  private void Start()
  {
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();
    _gameboardManager = GameboardManager.Instance;
  }

  private void OnDestroy()
  {
    Debug.Log("network mananger was destoyed :(");
    _roomHandlersRegistered = false;
    if (NullCheckRoom())
      return;
    _room.Leave();
  }

  public ColyseusRoom<GameState> GameRoom {
    get {
      return _room;
    }
  }

  public async Task CreateRoom(int time)
  {
    _room = await _client.Create<GameState>(GetRoomName(time));
    InitializeGame();
  }

  public async Task JoinRoom(string roomId)
  {
    _room = await _client.JoinById<GameState>(roomId);
    InitializeGame();
  }

  public async Task JoinOrCreateRoom(int time)
  {
    _room = await _client.JoinOrCreate<GameState>(GetRoomName(time));
    InitializeGame();
  }

  public void LeaveRoom()
  {
    if (NullCheckRoom())
      return;
    _room.Leave();
  }

  public void ReconnectToRoom()
  {
    if (NullCheckRoom())
      return;
    // _room.Connect();
    Debug.Log("Reconnecting to room");
  }

  private string GetRoomName(int time)
  {
    switch (time) {
      case 5:
        return "classic_five";
      case 10:
        return "classic_ten";
      case 15:
        return "classic_fifteen";
      case 20:
        return "classic_twenty";
      case 30:
        return "classic_thirty";
      default:
        Debug.LogError("Invalid Time Option: " + time);
        return null;
    }
  }

  private async void InitializeGame()
  {
    // GameboardManager.Instance.GamePieces.Clear();
    _boardCreated = false;
    _roomHandlersRegistered = false;
    PlayerId = 0;
    RegisterRoomHandlers();
    await GetPlayerIdAsync();
    GetClientId();
    GetRoomId();
    InitializeUIListeners();
    GamePieceManager.Instance.SetCatType(PlayerId);
    GamePieceManager.Instance.SetKittenType(PlayerId);
    OnGameInitialized?.Invoke();
  }

  private async Task<int> GetPlayerIdAsync()
  {
    while (PlayerId == 0) {
      await Task.Delay(100);
    }

    return PlayerId;
  }

  private void InitializeClient()
  {
    
    _client = new ColyseusClient("ws://localhost:2567");
  }

  private void RegisterRoomHandlers()
  {
    if (NullCheckRoom())
      return;
    if (_roomHandlersRegistered)
      return;

    _room.State.tiles.OnChange((cur, prev) =>
                               {
                                 if (_room.State.tiles == null)
                                   return;

                                 if (_boardCreated)
                                   return;
                                 _boardCreated = true;
                                 OnBoardCreated?.Invoke(_room.State);
                               });

    _room.State.gamePieces.OnAdd(
        (index, piece) =>
        {
          GamePiece gamePiece = Instantiate(ResourceManager.Instance.OrangeKittenPrefab,
                                            Vector3.zero, Quaternion.identity)
                                    .AddComponent<GamePiece>();
          GameboardManager.Instance.GamePieces.Add(index, gamePiece);
          gamePiece.Initialize(piece);
          piece.OnCoordinateChange((cur, prev) =>
                                   { gamePiece.HandleCoordinateChange(cur, prev); });
          piece.OnTypeChange((cur, prev) =>
                             { gamePiece.HandleTypeChange(cur, prev); });
        });

    _room.State.gamePieces.OnRemove((index, _piece) =>
                                    {
                                      GamePiece gamePiece =
                                          GameboardManager.Instance.GamePieces[index];
                                      GameboardManager.Instance.GamePieces.Remove(index);
                                      // Destroy(gamePiece.gameObject);
                                      // gamePiece.GetComponent<SpriteRenderer>().enabled = false;
                                    });

    _room.State.OnWinnerChange((value, prev) => OnPlayerWin?.Invoke(value));

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

    // Room messages
    _room.OnMessage<string>("choosePieceToEvolve",
                            (msg) => _gameboardManager.SelectTadpoleToEvolve());
    _room.OnMessage<int>("playerId", (id) => PlayerId = id);
    _room.OnMessage<string>("roomInitialized", (msg) => OnBoardCreated?.Invoke(_room.State));

    _roomHandlersRegistered = true;
  }

  public void SendEvolvingTadpole(int x, int y)
  {
    if (NullCheckRoom())
      return;
    _room.Send("evolveTadpole", new { x, y, playerId = PlayerId });
  }

  public void SendPieceMoved(GamePiece piece)
  {
    if (NullCheckRoom())
      return;
    _room.Send("pieceMoved",
               new { x = piece.Coordinate.x, y = piece.Coordinate.y, playerId = PlayerId });
  }

  public bool IsPlayerTurn()
  {
    if (NullCheckRoom())
      return false;
    return _room.State.currentPlayer == PlayerId;
  }

  public void PlacePiece(int x, int y, string type)
  {
    if (NullCheckRoom())
      return;
    _room.Send("placePiece", new { x, y, type, playerId = PlayerId });
  }

  private void InitializeUIListeners()
  {
    if (NullCheckRoom())
      return;
    Debug.Log("Room is not null");
    switch (PlayerId) {
      case 1:
        _room.State.playerOne.hand.OnKittensChange(
            (cur, prev) => OnHandChanged?.Invoke(_room.State.playerOne.hand));
        _room.State.playerOne.hand.OnCatsChange(
            (cur, prev) => OnHandChanged?.Invoke(_room.State.playerOne.hand));
        break;
      case 2:
        _room.State.playerTwo.hand.OnKittensChange(
            (cur, prev) => OnHandChanged?.Invoke(_room.State.playerTwo.hand));
        _room.State.playerTwo.hand.OnCatsChange(
            (cur, prev) => OnHandChanged?.Invoke(_room.State.playerTwo.hand));
        break;
      default:
        Debug.LogError("Player id is not set");
        break;
    }
  }

  private void GetClientId()
  {
    if (NullCheckRoom())
      return;
    ClientId = _room.SessionId;
  }

  private void GetRoomId()
  {
    if (NullCheckRoom())
      return;
    RoomId = _room.RoomId;
  }

  private bool NullCheckRoom()
  {
    if (_room != null)
      return false;
    Debug.Log("Room is null");
    return true;
  }
}
