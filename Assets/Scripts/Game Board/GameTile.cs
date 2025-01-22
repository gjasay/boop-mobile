using UnityEngine;

public class GameTile : MonoBehaviour
{
    public Vector2Int ArrayPosition { get; set; }
    public GamePiece CurrentlyHeldPiece { get; set; }

    private GameboardManager _gameboardManager;

    void Start()
    {
        _gameboardManager = GameboardManager.Instance;
        transform.SetParent(_gameboardManager.transform);
    }

    void Update()
    {
        SetLastTouchedTile();
    }

    public void PlacePiece(int playerId, string pieceType)
    {
        if (CurrentlyHeldPiece != null && CurrentlyHeldPiece.clientPlacement) Destroy(CurrentlyHeldPiece.gameObject);
        GameObject prefab;
        switch (pieceType)
        {
            case "kitten" when playerId == NetworkManager.Instance.PlayerId:
                prefab = ResourceManager.Instance.GetPrefab(GamePieceManager.Instance.ClientKittenType);
                break;
            case "cat" when playerId == NetworkManager.Instance.PlayerId:
                prefab = ResourceManager.Instance.GetPrefab(GamePieceManager.Instance.ClientCatType);
                break;
            case "kitten" when playerId != NetworkManager.Instance.PlayerId:
                prefab = ResourceManager.Instance.GetPrefab(GamePieceManager.Instance.OpponentKittenType);
                break;
            case "cat" when playerId != NetworkManager.Instance.PlayerId:
                prefab = ResourceManager.Instance.GetPrefab(GamePieceManager.Instance.OpponentCatType);
                break;
            default:
                Debug.LogError("Invalid game piece type: " + pieceType);
                return;
        }
        GameObject gamePiece = Instantiate(prefab, transform.position, Quaternion.identity);
        GamePiece piece = gamePiece.AddComponent<GamePiece>();
        piece.SetTilePlacement(this);
    }

    private void SetLastTouchedTile()
    {
        if (Input.touchCount > 0)
        {
            // Get the touch position
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            // Get all colliders at the touch position
            Collider2D[] colliders = Physics2D.OverlapPointAll(touchPosition);

            // Check if the touch position is over the game tile
            foreach (Collider2D collider in colliders)
            {
                if (collider == GetComponent<Collider2D>())
                {
                    _gameboardManager.LastTouchedGameTile = this;
                    break;
                }
            }
        }
    }
}
