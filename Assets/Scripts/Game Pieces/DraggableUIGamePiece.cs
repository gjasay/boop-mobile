using UnityEngine;


public enum GamePieceType
{
    OrangeTadpole,
    PurpleTadpole,
    OrangeFrog,
    PurpleFrog
}

public class DraggableUIGamePiece : MonoBehaviour
{
    [SerializeField] private GamePieceType gamePieceType; //The type of game piece
    private SpriteRenderer _spriteRenderer; //Reference to the sprite renderer
    private GameObject _prefab; //Reference to the prefab to be instantiated
    private bool _isDragging; //True if the game piece is being dragged

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        SetPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        DragPiece();
    }

    /*-----------------------------------------------------------------
    * This method changes the sprite depending on the game piece type
    ------------------------------------------------------------------*/
    private void SetSprite()
    {
        switch (gamePieceType)
        {
            case GamePieceType.OrangeTadpole:
                _spriteRenderer.sprite = Resources.Load<Sprite>("Art/orange_tadpole");
                break;
            case GamePieceType.PurpleTadpole:
                _spriteRenderer.sprite = Resources.Load<Sprite>("Art/purple_tadpole");
                break;
            case GamePieceType.OrangeFrog:
                _spriteRenderer.sprite = Resources.Load<Sprite>("Art/orange_frog");
                break;
            case GamePieceType.PurpleFrog:
                _spriteRenderer.sprite = Resources.Load<Sprite>("Art/purple_frog");
                break;
        }
    }

    /*-----------------------------------------------------------------------------------
     * This method sets the prefab to be instantiated depending on the game piece type
    -----------------------------------------------------------------------------------*/
    private void SetPrefab()
    {
        switch (gamePieceType)
        {
            case GamePieceType.OrangeTadpole:
                _prefab = Resources.Load<GameObject>("Prefabs/Orange Tadpole");
                break;
            case GamePieceType.PurpleTadpole:
                _prefab = Resources.Load<GameObject>("Prefabs/Purple Tadpole");
                break;
            case GamePieceType.OrangeFrog:
                _prefab = Resources.Load<GameObject>("Prefabs/Orange Frog");
                break;
            case GamePieceType.PurpleFrog:
                _prefab = Resources.Load<GameObject>("Prefabs/Purple Frog");
                break;
        }
    }

    /*--------------------------------------------------------------
     * This method is called when the game piece is tapped
     * Instantiate a new game piece that can be dragged on the game board
     * That game piece is then snapped to the nearest game tile
     ---------------------------------------------------------------*/
     private void DragPiece()
     {
        if (DetectTouch() && !_isDragging)
        {
            GameObject newGamePiece = Instantiate(_prefab, transform.position, Quaternion.identity);
            newGamePiece.AddComponent<PieceDragging>();
            _isDragging = true;
        }
     }

     /*--------------------------------------------------------------------------
      * Detect if the player has touched the game piece
      * @return - True if the player has touched the game piece, false otherwise
      ---------------------------------------------------------------------------*/
     private bool DetectTouch()
     {
         if (Input.touchCount > 0)
         {
             //Get the touch position
             Touch touch = Input.GetTouch(0);
             Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
             touchPosition.z = 0;

             //Check if the touch position is over the game piece
             if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
             {
                 return true;
             }
         }
         return false;
     }
}