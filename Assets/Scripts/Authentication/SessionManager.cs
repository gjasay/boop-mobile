using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;
    public string AuthToken { get; private set; }
    public string UserId { get; private set; }

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
    }

    public void SetSession(string authToken, string userId)
    {
        AuthToken = authToken;
        UserId = userId;
    }
    
}
