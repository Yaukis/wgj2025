using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Utils.EventBus;
using Cursor = UnityEngine.UIElements.Cursor;

public class EndScreenManager : MonoSingleton<EndScreenManager>
{
    private UIDocument _uiDocument;
    
    private VisualElement _rootElement;

    protected override void Awake()
    {
        base.Awake();
        
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null) return;
        
        _rootElement = _uiDocument.rootVisualElement.Q<VisualElement>("root");
    }

    private void Start()
    {
        EventBus<OnGameOverEvent>.AddListener(new EventBinding<OnGameOverEvent>(OnGameOver));
    }
    
    private void OnDestroy()
    {
        EventBus<OnGameOverEvent>.RemoveListener(new EventBinding<OnGameOverEvent>(OnGameOver));
    }
    
    private void OnGameOver()
    {
        if (_rootElement == null) return;
        
        _rootElement.style.opacity = 1;
    }
}