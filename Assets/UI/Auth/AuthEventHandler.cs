using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AuthEventHandler : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private Button _signInButton;
    private Button _guestButton;

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        
        _signInButton = _root.Q<Button>("sign-in");
        _guestButton = _root.Q<Button>("guest");
        
        // Button handlers

        _signInButton.clicked += async () => {
            await AuthManager.Instance.SignIn();
            SceneManager.LoadScene(1);
        };
        _guestButton.clicked += async () =>
        {
            await AuthManager.Instance.SignInAsGuest();
            SceneManager.LoadScene(1);
        };
    }
}
