using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    private async void Awake()
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

        await UnityServices.InitializeAsync();
        PlayerAccountService.Instance.SignedIn += SignedIn;
    }

    private async void SignedIn()
    {
        try
        {
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
            CheckFirstSignIn();
            SceneManager.LoadScene(1);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public async Task SignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    public async Task SignInAsGuest()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    
    private async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void OnDestroy()
    {
        PlayerAccountService.Instance.SignedIn -= SignedIn;
    }

    private void CheckFirstSignIn()
    {
        if (AuthenticationService.Instance.PlayerName == null)
        {
            Debug.Log("This is the first sign-in.");
        }
    }
}