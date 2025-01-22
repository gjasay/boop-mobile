using UnityEngine;

public enum GamePieceType
{
  OrangeKitten,
  GrayKitten,
  OrangeCat,
  GrayCat
}

public class GamePieceManager : MonoBehaviour
{
  public static GamePieceManager Instance { get; private set; } //Singleton instance

  //Properties
  public GamePieceType ClientKittenType { get; private set; } //The type of tadpole game piece
  public GamePieceType ClientCatType { get; private set; } //The type of frog game piece
  public GamePieceType OpponentKittenType { get; private set; } //The type of tadpole game piece for the opponent
  public GamePieceType OpponentCatType { get; private set; } //The type of frog game piece for the opponent

  private MainUIEventHandler _uiManager; //Reference to the main UI event handler

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

  private void Start()
  {
    _uiManager = GameObject.Find("GameUI").GetComponent<MainUIEventHandler>();
  }

  public void SetKittenType(int playerId)
  {
    if (playerId == 1)
    {
      ClientKittenType = GamePieceType.OrangeKitten;
      OpponentKittenType = GamePieceType.GrayKitten;
    }
    else
    {
      ClientKittenType = GamePieceType.GrayKitten;
      OpponentKittenType = GamePieceType.OrangeKitten;
    }

    // _uiManager.SetUIGamePieces();
  }

  public void SetCatType(int playerId)
  {
    if (playerId == 1)
    {
      ClientCatType = GamePieceType.OrangeCat;
      OpponentCatType = GamePieceType.GrayCat;
    }
    else
    {
      ClientCatType = GamePieceType.GrayCat;
      OpponentCatType = GamePieceType.OrangeCat;
    }
  }
}
