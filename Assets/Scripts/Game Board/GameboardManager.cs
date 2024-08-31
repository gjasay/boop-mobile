using Colyseus;
using UnityEngine;

public class GameboardManager : MonoBehaviour
{
    public GameTile SelectedGameTile { get; set; } // The currently selected game tile
    [SerializeField] private GameObject _gameTilePrefab; //Reference to the GameTile prefab
    [SerializeField] private GameObject _orangeTadpolePrefab; //Reference to the orange tadpole prefab
    [SerializeField] private GameObject _purpleTadpolePrefab; //Reference to the purple tadpole prefab
    private GameObject _selectedGamePiecePrefab; //Reference to the selected game piece prefab
    private GameTile[,] _gameTiles; //2D array of GameTile objects
    private ColyseusClient _client; //Reference to the Colyseus client
    private ColyseusRoom<GameState> _room; //Reference to the Game room
    private string _clientId; // The client id of the player
    private string _roomId; // The room id of the room
    private int _playerId; // The player id of the player (1 or 2)

    private void Start()
    {
        CreateRoom();

        //Testing
        _selectedGamePiecePrefab = _orangeTadpolePrefab;
    }

    /*---------------------------------------------------------
     * Create a game board with the specified width and height
     * @param width - The width of the game board
     * @param height - The height of the game board
     * @param tileSize - The size of each tile in the game board
     ----------------------------------------------------------*/
    public void CreateGameboard(int width = 6, int height = 6, float tileSize = 0.65f)
    {
        _gameTiles = new GameTile[width, height];

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

                _gameTiles[x, y] = gameTile;
            }
        }
    }

    /*--------------------------------------------------------------
     * This method is called when the place button is pressed
     * Place a game piece on the selected game tile
     * Communicate to the selected game tile that it is holding a game piece
     -----------------------------------------------------------------------*/
    public void PlaceGamePiece()
    {
        if (SelectedGameTile == null || SelectedGameTile.CurrentlyHeldPiece != null) return;

        GameObject gamePiece =  Instantiate(_selectedGamePiecePrefab, SelectedGameTile.transform.position, Quaternion.identity);

        SelectedGameTile.CurrentlyHeldPiece = gamePiece.GetComponent<GamePieceScheme>();
    }

    /*---------------------------------------
     * Create a new room on the server
     ----------------------------------------*/
    async public void CreateRoom()
    {
        _client = new ColyseusClient("ws://localhost:2567"); //Create a new Colyseus client
        _room = await _client.Create<GameState>("my_room"); //Create a new room on the server

        await _room.Send("createRoom"); //Send a message to the server to create a room

        GetClientId();
        GetRoomId();

        CreateGameboard(); //Create a 6x6 game board

        _playerId = 1;
    }

    /*---------------------------------------
     * Join an existing room on the server
     * @param roomId - The id of the room to join
     ----------------------------------------*/
    async public void JoinRoom(string roomId)
    {
        _client = new ColyseusClient("ws://localhost:2567"); //Create a new Colyseus client
        _room = await _client.JoinById<GameState>(roomId); //Join a colyseus room

        await _room.Send("joinRoom"); //Send a message to the server to join the room

        GetClientId(); 
        CreateGameboard(); //Create a 6x6 game board

        _playerId = 2; //Set the player id to 2
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
            Debug.Log("Room ID: " + _roomId);
        });
    }
}
