using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomCodeText; // Reference to the room code text
    [SerializeField] private TMP_InputField _roomCodeInput; // Reference to the room code input field
    [SerializeField] private TMP_Text _numberOfFrogs; // Reference to the number of frogs text
    [SerializeField] private TMP_Text _numberOfTadpoles; // Reference to the number of tadpoles text

    private void Start()
    {
        if (NetworkManager.Instance == null)
        {
            Debug.LogError("NetworkManager is not initialized");
            return;
        }
        NetworkManager.Instance.OnUIChanged += UpdateUI;
    }

    private void UpdateUI(HandState handState)
    {
        if (handState == null) return;
        _numberOfFrogs.text = handState.frogs.ToString();
        _numberOfTadpoles.text = handState.tadpoles.ToString();
    }

    public void SetRoomCode(string roomCode)
    {
        _roomCodeText.text = "Room Code: " + roomCode;
    }

    public void DisableRoomCodeText()
    {
        _roomCodeText.gameObject.SetActive(false);
    }

    public string GetRoomCode()
    {
        return _roomCodeInput.text;
    }
}
