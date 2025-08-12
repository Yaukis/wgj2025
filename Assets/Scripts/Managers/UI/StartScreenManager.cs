using System.Collections.Generic;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.UIElements;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private SoundID buttonClickSound;
    
    private VisualElement _rootElement;
    private VisualElement _mainMenuContainer;
    private VisualElement _settingsContainer;
    
    // Start menu
    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;
    
    // Settings menu
    private Slider _generalVolumeSlider;
    private Slider _musicVolumeSlider;
    private Slider _sfxVolumeSlider;
    private Slider _mouseSensitivitySlider;
    private Button _backButton;
    
    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("StartScreenManager: UIDocument component not found.");
            return;
        }

        _rootElement = uiDocument.rootVisualElement.Q<VisualElement>("root");
        _mainMenuContainer = _rootElement.Q<VisualElement>("mainMenuContainer");
        _settingsContainer = _rootElement.Q<VisualElement>("settingsContainer");
        
        if (_rootElement == null)
        {
            Debug.LogError("StartScreenManager: Root element not found.");
            return;
        }
        
        // Find all Button elements and register the ClickEvent and MouseEnterEvent callbacks
        var buttons = new List<Button>();
        buttons.AddRange(_rootElement.Query<Button>().ToList());

        foreach (var button in buttons)
        {
            button.RegisterCallback<ClickEvent>(OnButtonPress, TrickleDown.TrickleDown);
        }
        
        // Start menu
        _startButton = _mainMenuContainer.Q<Button>("startButton");
        _settingsButton = _mainMenuContainer.Q<Button>("settingsButton");
        _exitButton = _mainMenuContainer.Q<Button>("exitButton");

        if (_startButton != null)
        {
            _startButton.clicked += OnStartButtonClicked;
        }
        
        if (_settingsButton != null)
        {
            _settingsButton.clicked += ToggleSettingsMenu;
        }
        
        if (_exitButton != null)
        {
            _exitButton.clicked += OnExitButtonClicked;
        }
        
        // Settings menu
        _generalVolumeSlider = _settingsContainer.Q<Slider>("generalVolumeSlider");
        _musicVolumeSlider = _settingsContainer.Q<Slider>("musicVolumeSlider");
        _sfxVolumeSlider = _settingsContainer.Q<Slider>("sfxVolumeSlider");
        _mouseSensitivitySlider = _settingsContainer.Q<Slider>("mouseSensitivitySlider");
        _backButton = _settingsContainer.Q<Button>("backButton");
        
        _generalVolumeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            AudioManager.Instance.SetVolume(evt.newValue);
        });
        
        _musicVolumeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            AudioManager.Instance.SetMusicVolume(evt.newValue);
        });
        
        _sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            AudioManager.Instance.SetSFXVolume(evt.newValue);
        });
        
        _mouseSensitivitySlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            GameManager.Instance.SetMouseSensitivity(evt.newValue);
        });
        
        if (_backButton != null)
        {
            _backButton.clicked += ToggleSettingsMenu;
        }
    }
    
    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked. Starting the game...");
        GameManager.Instance.StartGame();
        _rootElement.style.display = DisplayStyle.None;
    }
    
    private void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked. Exiting the game...");
        Application.Quit();
    }
    
    private void ToggleSettingsMenu()
    {
        if (_settingsContainer != null)
        {
            _mainMenuContainer.style.opacity = _mainMenuContainer.style.opacity == 0 ? 1 : 0;
            _settingsContainer.style.translate = _mainMenuContainer.style.opacity == 0
                ? new StyleTranslate(new Translate(0, 0)) 
                : new StyleTranslate(new Translate(0, 1080));
        }
        else
        {
            Debug.LogError("Settings menu element not found.");
        }
    }
    
    private void OnButtonPress(ClickEvent evt)
    {
        BroAudio.Play(buttonClickSound);
    }
}