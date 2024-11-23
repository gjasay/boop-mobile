using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuEventHandler : MonoBehaviour
{
  private UIDocument _document;
  private GameManager _gameManager;

  private Button _createButton;
  private Button _createBackButton;
  private Button _joinButton;
  private Button _joinBackButton;
  private Button _findButton;
  private Button _findBackButton;
  private Button _settingsButton;
  private Button _settingsBackButton;
  
  private Button _createSubmitButton;
  private Button _joinSubmitButton;
  private Button _findSubmitButton;

  private Box _createBox;
  private Box _joinBox;
  private Box _findBox;
  private Box _settingsBox;
  
  private RadioButtonGroup _timerRadioGroup;
  private int _timerValue;
  
  private Dictionary<Button, Box> _buttonBoxMap;
  private Dictionary<int, int> _timerValueMap;
  
  private void Start()
  {
    _gameManager = GameManager.Instance;
    if (NetworkManager.Instance == null)
    {
      Debug.LogError("NetworkManager is not initialized");
      return;
    }
  }

  private void OnEnable()
  {
    _document = GetComponent<UIDocument>();

    _createButton = _document.rootVisualElement.Q<Button>("create-button");
    _createBackButton = _document.rootVisualElement.Q<Button>("create-back-button");
    _joinButton = _document.rootVisualElement.Q<Button>("join-button");
    _joinBackButton = _document.rootVisualElement.Q<Button>("join-back-button");
    _settingsButton = _document.rootVisualElement.Q<Button>("settings-button");
    _findButton = _document.rootVisualElement.Q<Button>("find-button");
    _findBackButton = _document.rootVisualElement.Q<Button>("find-back-button");
    _settingsBackButton = _document.rootVisualElement.Q<Button>("settings-back-button");
    _createSubmitButton = _document.rootVisualElement.Q<Button>("create-submit-button");
    _joinSubmitButton = _document.rootVisualElement.Q<Button>("join-submit-button");
    _findSubmitButton = _document.rootVisualElement.Q<Button>("find-submit-button");

    _createBox = _document.rootVisualElement.Q<Box>("create-box");
    _joinBox = _document.rootVisualElement.Q<Box>("join-box");
    _findBox = _document.rootVisualElement.Q<Box>("find-box");
    _settingsBox = _document.rootVisualElement.Q<Box>("settings-box");
    
    _timerRadioGroup = _document.rootVisualElement.Q<RadioButtonGroup>("timer-radio-group");
    
    _buttonBoxMap = new Dictionary<Button, Box>()
    {
      { _createButton, _createBox },
      { _createBackButton, _createBox },
      { _joinButton, _joinBox },
      { _joinBackButton, _joinBox },
      { _findButton, _findBox },
      { _findBackButton, _findBox },
      { _settingsButton, _settingsBox },
      { _settingsBackButton, _settingsBox }
    };
    
    _timerValueMap = new Dictionary<int, int>()
    {
      { 0, 5 },
      { 1, 10 },
      { 2, 15 },
      { 3, 20 },
      { 4, 30 },
    };

    foreach (Button button in _buttonBoxMap.Keys)
    {
      if (button == null || _buttonBoxMap[button] == null) return;
      button.clicked += () => { _buttonBoxMap[button].ToggleInClassList("visible"); };
    }
    
    _createSubmitButton.clicked += () =>
    {
      _gameManager.CreateGame(_timerValueMap[_timerValue] * 60);
    };
    
    _joinSubmitButton.clicked += () =>
    {
      _gameManager.JoinGame();
    };
    
    _timerRadioGroup.RegisterValueChangedCallback(evt =>
    {
      _timerValue = evt.newValue;
      Debug.Log($"Timer value: {_timerValue}");
    });
  }
}