using UnityEngine;

public class PieceDragging : MonoBehaviour
{
    //Update is called once per frame
    void Update()
    {
        transform.position = GetTouchPosition();
    }

    /*------------------------
     * Get the touch position
     -------------------------*/
    private Vector2 GetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return Camera.main.ScreenToWorldPoint(touch.position);
        }

        return GetNearestGameTilePosition();
    }

    /*---------------------------------------------
     * Get the position of the nearest game tile
     * @return Vector2 - The position of the nearest game tile
     ----------------------------------------------*/
    private Vector2 GetNearestGameTilePosition()
    {
        GameTile gameTile = GameObject.Find("GameboardManager").GetComponent<GameboardManager>().LastTouchedGameTile;

        if (gameTile == null) return Vector2.zero;

        return gameTile.transform.position;
    }
}
