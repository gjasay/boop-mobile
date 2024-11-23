using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set;} //Singleton instance

    [Header("Sprites")]
    public Sprite OrangeKittenSprite; //Reference to the Orange Tadpole sprite
    public Sprite GrayKittenSprite; //Reference to the Purple Tadpole sprite
    public Sprite OrangeCatSprite; //Reference to the Orange Frog sprite
    public Sprite GrayCatSprite; //Reference to the Purple Frog sprite

    [Header("Prefabs")]
    public GameObject OrangeKittenPrefab; //Reference to the Orange Tadpole prefab
    public GameObject GrayKittenPrefab; //Reference to the Purple Tadpole prefab
    public GameObject OrangeCatPrefab; //Reference to the Orange Frog prefab
    public GameObject GrayCatPrefab; //Reference to the Purple Frog prefab

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

    /*-------------------------------------------------------
    * This method returns the sprite of the game piece type
    * @param type - The game piece type
    * @return Sprite - The sprite of the game piece type
    ---------------------------------------------------------*/
    public Sprite GetSprite(GamePieceType type)
    {
        switch (type)
        {
            case GamePieceType.OrangeTadpole:
                return OrangeKittenSprite;
            case GamePieceType.PurpleTadpole:
                return GrayKittenSprite;
            case GamePieceType.OrangeFrog:
                return OrangeCatSprite;
            case GamePieceType.PurpleFrog:
                return GrayCatSprite;
            default:
                Debug.LogError("Invalid game piece type: " + type);
                return null;
        }
    }

    /*-------------------------------------------------------
    * This method returns the prefab of the game piece type
    * @param type - The game piece type
    * @return GameObject - The prefab of the game piece type
    ---------------------------------------------------------*/
    public GameObject GetPrefab(GamePieceType type)
    {
        switch (type)
        {
            case GamePieceType.OrangeTadpole:
                return OrangeKittenPrefab;
            case GamePieceType.PurpleTadpole:
                return GrayKittenPrefab;
            case GamePieceType.OrangeFrog:
                return OrangeCatPrefab;
            case GamePieceType.PurpleFrog:
                return GrayCatPrefab;
            default:
                Debug.LogError("Invalid game piece type: " + type);
                return null;
        }
    }
}
