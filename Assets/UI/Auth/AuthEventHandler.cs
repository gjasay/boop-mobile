using UnityEngine;
using UnityEngine.UIElements;

public class AuthEventHandler : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private Button _guestButton;

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _guestButton = _root.Q<Button>("guest");

        _guestButton.clicked += () =>
        {
            AuthManager.Instance.SignInAsGuest();
        };
    }
}