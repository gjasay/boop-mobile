using System.Collections.Generic;
using UnityEngine;

// Other needed dependencies
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class FacebookAuth : MonoBehaviour
{
    public static FacebookAuth Instance;
    
    [SerializeField] private string token;
    [SerializeField] private string error;

    // Awake function from Unity's MonoBehaviour
    void Awake()
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
        
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void Login()
    {
        // Define the permissions
        var perms = new List<string>() { "public_profile" };

        FB.LogInWithReadPermissions(perms, result =>
        {
            if (FB.IsLoggedIn)
            {
                token = AccessToken.CurrentAccessToken.TokenString;
                Debug.Log($"Facebook Login token: {token}");
                SceneManager.LoadScene(1);
            }
            else
            {
                error = "User cancelled login";
                Debug.Log("[Facebook Login] User cancelled login");
            }
        });
    }
}