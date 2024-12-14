using UnityEngine;
using UnityEngine.UIElements;

public class AuthEventHandler : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private Button _guestButton;
    private Button _facebookButton;
    private Button _googleButton;
    private Button _appleButton;

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        
        _guestButton = _root.Q<Button>("guest");
        _facebookButton = _root.Q<Button>("fb");
        _googleButton = _root.Q<Button>("google");
        _appleButton = _root.Q<Button>("apple");
        
        // Button handlers
        _guestButton.clicked += () =>
        {
            AuthManager.Instance.SignIn();
        };

        _facebookButton.clicked += () =>
        {
            FacebookAuth.Instance.Login();
        };

        _googleButton.clicked += () =>
        {
            GoogleAuth.Instance.Login();
        };
    }
}