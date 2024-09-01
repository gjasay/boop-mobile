using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Reference to the UIManager
    private GameboardManager _gameboardManager;

    void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    public void CreateGame()
    {
        SceneManager.LoadScene("Main");

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name != "Main") return;
            
            _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>();
            if (_gameboardManager == null) return;
            _gameboardManager.CreateRoom();
        };
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Main");

        string roomCode = _uiManager.GetRoomCode();   

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name != "Main") return;
            
            _gameboardManager = GameObject.Find("GameboardManager").GetComponent<GameboardManager>();
            if (_gameboardManager == null) return;
            _gameboardManager.JoinRoom(roomCode);
        };
    }
}