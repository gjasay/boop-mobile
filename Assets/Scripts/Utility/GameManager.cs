using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private NetworkManager _networkManager; // Reference to the NetworkManager
    public static GameManager Instance { get; private set; } // Singleton instance

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

    void Start()
    {
        _networkManager = NetworkManager.Instance;
    }

    public void CreateGame(int time)
    {
        SceneManager.LoadScene("Main");

        SceneManager.sceneLoaded += async (scene, mode) =>
        {
            if (scene.name != "Main") return;

            _networkManager = NetworkManager.Instance;

            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager is null");
                return;
            }

            await _networkManager.CreateRoom("my_room", time);
        };
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Main");

        // string roomCode = _uiManager.GetRoomCode();   

        SceneManager.sceneLoaded += async (scene, mode) =>
        {
            if (scene.name != "Main") return;

            _networkManager = NetworkManager.Instance;

            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager is null");
                return;
            }

            // Debug.Log("Joining room: " + roomCode);

            await _networkManager.JoinRoom("none yet");
        };
    }

    public void FindGame()
    {
        SceneManager.LoadScene("Main");

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name != "Main") return;

            _networkManager = NetworkManager.Instance;

            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager is null");
                return;
            }

            _ = _networkManager.JoinOrCreateRoom();
        };
    }
}