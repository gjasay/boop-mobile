using UnityEngine;
using UnityEngine.UIElements;

public class MenuEventHandler : MonoBehaviour
{
    private UIDocument _document;

    private void Start()
    {
        if (NetworkManager.Instance == null)
        {
            Debug.LogError("NetworkManager is not initialized");
            return;
        }
    }

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();

        Button createButton = _document.rootVisualElement.Q<Button>("create-button");
        Button joinButton = _document.rootVisualElement.Q<Button>("join-button");

        if (createButton != null)
        {
            createButton.clicked += () =>
            {
                GameManager.Instance.CreateGame();
            };
        }

        if (joinButton != null)
        {
            joinButton.clicked += () =>
            {
                Debug.Log("Joining game");
                GameManager.Instance.JoinGame();
            };
        }
    }
}
