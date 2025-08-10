using UnityEngine;
using UnityEngine.UIElements;

public class StartScreenManager : MonoBehaviour
{
    private VisualElement _rootElement;
    
    private Button _startButton;
    private Button _exitButton;
    
    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("StartScreenManager: UIDocument component not found.");
            return;
        }

        _rootElement = uiDocument.rootVisualElement.Q<VisualElement>("root");
        if (_rootElement == null)
        {
            Debug.LogError("StartScreenManager: Root element not found.");
            return;
        }

        _startButton = _rootElement.Q<Button>("startButton");
        _exitButton = _rootElement.Q<Button>("exitButton");

        if (_startButton != null)
        {
            _startButton.clicked += OnStartButtonClicked;
        }
        
        if (_exitButton != null)
        {
            _exitButton.clicked += OnExitButtonClicked;
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
}