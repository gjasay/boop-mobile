using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }  // Singleton instance

  private void Awake()
  {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  public void CreateGame(int time)
  {
    SceneManager.LoadScene("Main");

    SceneManager.sceneLoaded += async (scene, mode) =>
    {
      if (scene.name != "Main")
        return;

      await NetworkManager.Instance?.CreateRoom(time);
    };
  }

  public void JoinGame()
  {
    SceneManager.LoadScene("Main");

    // string roomCode = _uiManager.GetRoomCode();

    SceneManager.sceneLoaded += async (scene, mode) =>
    {
      if (scene.name != "Main")
        return;

      await NetworkManager.Instance?.JoinRoom("none yet");
    };
  }

  public void FindGame(int time)
  {
    SceneManager.LoadScene("Main");

    SceneManager.sceneLoaded += async (scene, mode) =>
    {
      if (scene.name != "Main") return;

      await NetworkManager.Instance.JoinOrCreateRoom(time);
    };
  }
}
