using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomCodeText; // Reference to the room code text
    [SerializeField] private TMP_InputField _roomCodeInput; // Reference to the room code input field


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
