using UnityEngine;

public class PieceDragging : MonoBehaviour
{
    public DraggableUIGamePiece UIGamePiece { get; set; } //The UI game piece I'm being dragged from
    private bool _isDragging = true; //True if the piece is being dragged

    //Update is called once per frame
    void Update()
    {
        UpdateCurrentPosition();
    }

    private void UpdateCurrentPosition()
    {
        if (_isDragging == false) return;

        Vector2? touchPosition = GetTouchPosition();
        
        if (touchPosition == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = new Vector3(touchPosition.Value.x, touchPosition.Value.y, 0);
        }
    }

    /*------------------------
     * Get the touch position
     -------------------------*/
    private Vector2? GetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                _isDragging = false;
                UIGamePiece.IsDragging = false;
                return GetNearestGameTilePosition();
            }

            return Camera.main.ScreenToWorldPoint(touch.position);
        }
        return GetNearestGameTilePosition();
    }

    /*---------------------------------------------
     * Get the position of the nearest game tile
     * @return Vector2 - The position of the nearest game tile
     ----------------------------------------------*/
    private Vector2? GetNearestGameTilePosition()
    {
        GameTile gameTile = GameObject.Find("GameboardManager").GetComponent<GameboardManager>().LastTouchedGameTile;

        if (gameTile == null) return null;

        return gameTile.transform.position;
    }
}