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

  public GamePieceType ClientTadpoleType { get; private set; } //The type of tadpole game piece
  public GamePieceType ClientFrogType { get; private set; } //The type of frog game piece
  public GamePieceType OpponentTadpoleType { get; private set; } //The type of tadpole game piece for the opponent
  public GamePieceType OpponentFrogType { get; private set; } //The type of frog game piece for the opponent

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
      ClientFrogType = GamePieceType.OrangeFrog;

      OpponentTadpoleType = GamePieceType.PurpleTadpole;
      OpponentFrogType = GamePieceType.PurpleFrog;
    }
    else
    {
      ClientTadpoleType = GamePieceType.PurpleTadpole;
      ClientFrogType = GamePieceType.PurpleFrog;

      OpponentTadpoleType = GamePieceType.OrangeTadpole;
      OpponentFrogType = GamePieceType.OrangeFrog;
    }
  }
}
