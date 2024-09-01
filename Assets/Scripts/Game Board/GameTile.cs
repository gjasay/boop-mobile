using UnityEngine;

public class GameTile : MonoBehaviour
{
    public GamePiece CurrentlyHeldPiece { get; set; } //The game piece currently held by the game tile
    private GameboardManager _gameboardManager; //Reference to the GameboardManager

    // Start is called before the first frame update
    private void Start()
    {
        _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>(); //Get a reference to the GameboardManager
        transform.SetParent(GameObject.Find("GameboardManager").transform); //Set the GameTile's parent to the GameboardManager
    }

    private void Update()
    {
        SetLastTouchedTile();
    }

    /*-------------------------------------------------
     * Detect if the game tile is being touched
     * @return true if the game tile is being touched
     --------------------------------------------------*/
    private void SetLastTouchedTile()
    {
        if (Input.touchCount > 0)
        {
            //Get the touch position
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            //Check if the touch position is over the game tile
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition))
            {
                _gameboardManager.LastTouchedGameTile = this;
            }
        }
    }
}