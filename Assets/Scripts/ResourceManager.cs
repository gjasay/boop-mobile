using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set;} //Singleton instance

    [Header("Sprites")]
    public Sprite OrangeTadpoleSprite; //Reference to the Orange Tadpole sprite
    public Sprite PurpleTadpoleSprite; //Reference to the Purple Tadpole sprite
    public Sprite OrangeFrogSprite; //Reference to the Orange Frog sprite
    public Sprite PurpleFrogSprite; //Reference to the Purple Frog sprite

    [Header("Prefabs")]
    public GameObject OrangeTadpolePrefab; //Reference to the Orange Tadpole prefab
    public GameObject PurpleTadpolePrefab; //Reference to the Purple Tadpole prefab
    public GameObject OrangeFrogPrefab; //Reference to the Orange Frog prefab
    public GameObject PurpleFrogPrefab; //Reference to the Purple Frog prefab

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
                return OrangeTadpoleSprite;
            case GamePieceType.PurpleTadpole:
                return PurpleTadpoleSprite;
            case GamePieceType.OrangeFrog:
                return OrangeFrogSprite;
            case GamePieceType.PurpleFrog:
                return PurpleFrogSprite;
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
                return OrangeTadpolePrefab;
            case GamePieceType.PurpleTadpole:
                return PurpleTadpolePrefab;
            case GamePieceType.OrangeFrog:
                return OrangeFrogPrefab;
            case GamePieceType.PurpleFrog:
                return PurpleFrogPrefab;
            default:
                Debug.LogError("Invalid game piece type: " + type);
                return null;
        }
    }
}
