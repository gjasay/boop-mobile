using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager; // Reference to the UIManager
    private NetworkManager _networkManager; // Reference to the NetworkManager

    void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        _networkManager = NetworkManager.Instance;
    }

    public void CreateGame()
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

            await _networkManager.CreateRoom("my_room");
        };
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Main");

        string roomCode = _uiManager.GetRoomCode();   

        SceneManager.sceneLoaded += async (scene, mode) =>
        {
            if (scene.name != "Main") return;

            _networkManager = NetworkManager.Instance;

            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager is null");
                return;
            }

            await _networkManager.JoinRoom(roomCode);
        };
    }
}