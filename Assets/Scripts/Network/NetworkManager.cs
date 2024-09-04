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
    public event Action<GamePieceState> OnTadpolePlaced; //Event that is triggered when the game state changes
    public event Action<GamePieceState> OnFrogPlaced; //Event that is triggered when the game state changes

    /* Private variables */
    private ColyseusClient _client; //Reference to the Colyseus client
    private ColyseusRoom<GameState> _room; //Reference to the Game room
    private UIManager _uiManager; //Reference to the UIManager

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

        GetClientId();
        GetRoomId();

        _uiManager.SetRoomCode(RoomId);

        PlayerId = 1;
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
        _room = await _client.JoinById<GameState>(roomId); //Join a colyseus room
        RegisterRoomHandlers(); //Register the room handlers

        GetClientId();
        GetRoomId();

        _uiManager.DisableRoomCodeText();

        PlayerId = 2;
        GamePieceManager.Instance.SetFrogType(PlayerId);
        GamePieceManager.Instance.SetTadpoleType(PlayerId);
    }

    /*------------------------------------------------
    * Send a message to the server to place a tadpole
    * @param gamePieceState - The game piece state
    --------------------------------------------------*/
    public async void SendTadpolePlacement(GamePieceState gamePieceState)
    {
        NullCheckRoom();
        await _room.Send("placeTadpole", gamePieceState);
    }

    /*------------------------------------------------
    * Send a message to the server to place a frog
    * @param gamePieceState - The game piece state
    --------------------------------------------------*/
    public async void SendFrogPlacement(GamePieceState gamePieceState)
    {
        NullCheckRoom();
        await _room.Send("placeFrog", gamePieceState);
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
        _room.OnMessage<GamePieceState>("tadpolePlaced", (message) =>
        {
            OnTadpolePlaced?.Invoke(message);
        });

        _room.OnMessage<GamePieceState>("frogPlaced", (message) =>
        {
            OnFrogPlaced?.Invoke(message);
        });
    }

    /*------------------
    * Get the client id
    --------------------*/
    private void GetClientId()
    {
        NullCheckRoom();
        ClientId = _room.SessionId;
    }

    /*------------------
    * Get the room id
    --------------------*/
    private void GetRoomId()
    {
        NullCheckRoom();
        RoomId = _room.RoomId;
    }

    /*--------------------------
    * Check if the room is null
    ----------------------------*/
    private void NullCheckRoom()
    {
        if (_room == null)
        {
            Debug.LogError("Room is null");
            return;
        }
    }

}
