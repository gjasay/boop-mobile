using UnityEngine;

public class GameboardManager : MonoBehaviour
{
    public GameTile SelectedGameTile { get; set; } // The currently selected game tile
    [SerializeField] private GameObject _gameTilePrefab; //Reference to the GameTile prefab
    [SerializeField] private GameObject _orangeTadpolePrefab; //Reference to the orange tadpole prefab
    [SerializeField] private GameObject _purpleTadpolePrefab; //Reference to the purple tadpole prefab
    private GameObject _selectedGamePiecePrefab; //Reference to the selected game piece prefab
    private GameTile[,] _gameTiles; //2D array of GameTile objects

    private void Start()
    {
        //Create a 6x6 game board
        CreateGameboard(6, 6);

        //Testing
        _selectedGamePiecePrefab = _orangeTadpolePrefab;
    }


    /*---------------------------------------------------------
     * Create a game board with the specified width and height
     * @param width The width of the game board
     * @param height The height of the game board
     * @param tileSize The size of each tile in the game board
     ----------------------------------------------------------*/
    public void CreateGameboard(int width, int height, float tileSize = 0.65f)
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

        SelectedGameTile.CurrentlyHeldPiece = gamePiece.GetComponent<GamePiece>();
    }

}
