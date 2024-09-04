using UnityEngine;

public enum GamePieceType
{
  OrangeTadpole,
  PurpleTadpole,
  OrangeFrog,
  PurpleFrog
}

public class GamePieceManager : MonoBehaviour
{
  public static GamePieceManager Instance { get; private set; } //Singleton instance

  //Properties
  public GamePieceType ClientTadpoleType { get; private set; } //The type of tadpole game piece
  public GamePieceType ClientFrogType { get; private set; } //The type of frog game piece
  public GamePieceType OpponentTadpoleType { get; private set; } //The type of tadpole game piece for the opponent
  public GamePieceType OpponentFrogType { get; private set; } //The type of frog game piece for the opponent

  [Header("Draggable UI Game Pieces")]
  [SerializeField] private DraggableUIGamePiece _tadpoleDraggable;
  [SerializeField] private DraggableUIGamePiece _frogDraggable;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  /*-------------------------------------------
  * Set the tadpole type based on the player id
  * @param playerId - The player id
  ---------------------------------------------*/
  public void SetTadpoleType(int playerId)
  {
    if (playerId == 1)
    {
      ClientTadpoleType = GamePieceType.OrangeTadpole;
      OpponentTadpoleType = GamePieceType.PurpleTadpole;
    }
    else
    {
      ClientTadpoleType = GamePieceType.PurpleTadpole;
      OpponentTadpoleType = GamePieceType.OrangeTadpole;
    }

    _frogDraggable.SetUIGamePieces();
    _tadpoleDraggable.SetUIGamePieces();
  }

  /*-------------------------------------------
  * Set the frog type based on the player id
  * @param playerId - The player id
  ---------------------------------------------*/
  public void SetFrogType(int playerId)
  {
    if (playerId == 1)
    {
      Debug.Log("Setting frog type to orange");
      ClientFrogType = GamePieceType.OrangeFrog;
      OpponentFrogType = GamePieceType.PurpleFrog;
    }
    else
    {
      ClientFrogType = GamePieceType.PurpleFrog;
      OpponentFrogType = GamePieceType.OrangeFrog;
    }
  }
}
