using UnityEngine;

public class GameTile : MonoBehaviour
{
  //Properties
  public Vector2Int ArrayPosition { get; set; } //The position of the game tile in the 2D array
  public PlacedPiece CurrentlyHeldPiece { get; set; } //The game piece currently held by the game tile

  //Private variables
  private GameboardManager _gameboardManager; //Reference to the GameboardManager

  // Start is called before the first frame update
  private void Start()
  {
    _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>(); //Get a reference to the GameboardManager
    transform.SetParent(GameObject.Find("GameboardManager").transform); //Set the GameTile's parent to the GameboardManager
  }

  // Update is called once per frame
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

      // Get all colliders at the touch position
      Collider2D[] colliders = Physics2D.OverlapPointAll(touchPosition);

      // Check if the touch position is over the game tile
      foreach (var collider in colliders)
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