using UnityEngine;

public class ResourceManager : MonoBehaviour
{
  public static ResourceManager Instance { get; private set; }  // Singleton instance

  [Header("Sprites")]
  public Sprite OrangeKittenSprite;  // Reference to the Orange Tadpole sprite
  public Sprite GrayKittenSprite;    // Reference to the Purple Tadpole sprite
  public Sprite OrangeCatSprite;     // Reference to the Orange Frog sprite
  public Sprite GrayCatSprite;       // Reference to the Purple Frog sprite

  [Header("Prefabs")]
  public GameObject OrangeKittenPrefab;  // Reference to the Orange Tadpole prefab
  public GameObject GrayKittenPrefab;    // Reference to the Purple Tadpole prefab
  public GameObject OrangeCatPrefab;     // Reference to the Orange Frog prefab
  public GameObject GrayCatPrefab;       // Reference to the Purple Frog prefab

  private void Awake()
  {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  public Sprite GetSprite(GamePieceType type)
  {
    switch (type) {
      case GamePieceType.OrangeKitten:
        return OrangeKittenSprite;
      case GamePieceType.GrayKitten:
        return GrayKittenSprite;
      case GamePieceType.OrangeCat:
        return OrangeCatSprite;
      case GamePieceType.GrayCat:
        return GrayCatSprite;
      default:
        Debug.LogError("Invalid game piece type: " + type);
        return null;
    }
  }

  public Sprite GetSprite(string type, int playerId)
  {
    switch (type) {
      case "kitten":
        return playerId == 1 ? OrangeKittenSprite : GrayKittenSprite;
      case "cat":
        return playerId == 1 ? OrangeCatSprite : GrayCatSprite;
      default:
        Debug.LogError("Invalid game piece type: " + type);
        return null;
    }
  }

  public GameObject GetPrefab(GamePieceType type)
  {
    switch (type) {
      case GamePieceType.OrangeKitten:
        return OrangeKittenPrefab;
      case GamePieceType.GrayKitten:
        return GrayKittenPrefab;
      case GamePieceType.OrangeCat:
        return OrangeCatPrefab;
      case GamePieceType.GrayCat:
        return GrayCatPrefab;
      default:
        Debug.LogError("Invalid game piece type: " + type);
        return null;
    }
  }

  public GameObject GetPrefab(string type, int playerId)
  {
    switch (type) {
      case "kitten":
        return playerId == 1 ? OrangeKittenPrefab : GrayKittenPrefab;
      case "cat":
        return playerId == 1 ? OrangeCatPrefab : GrayCatPrefab;
      default:
        Debug.LogError("Invalid game piece type: " + type);
        return null;
    }
  }
}
