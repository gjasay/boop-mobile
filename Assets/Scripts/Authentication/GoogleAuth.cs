using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleAuth : MonoBehaviour
{
    public static GoogleAuth Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    public void Login()
    {
        PlayGamesPlatform.Instance.Authenticate((success, error) =>
        {
            if (success)
            {
                Debug.Log("Login Success");
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.Log("Login Failed: " + error);
            }
        });
    }

    private void Initialize()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
}