using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

    async void Awake()
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
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async Task SignInAsGuest()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"PlayerID {AuthenticationService.Instance.PlayerId}: Signed in as guest");
        }
        catch (AuthenticationException e)
        {
            Debug.LogException(e);
        }
        catch (RequestFailedException e)
        {
            Debug.LogException(e);
        }
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            Debug.Log($"AccessToken: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += Debug.LogError;

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player has signed out successfully");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session has expired");
        };
    }
}
