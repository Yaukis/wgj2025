using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Utils.EventBus;

public class HUDManager : MonoSingleton<HUDManager>
{
    private UIDocument _uiDocument;
    
    private VisualElement _rootElement;
    
    private VisualElement _recipeBookContainerElement;
    private VisualElement _recipeBookNormalElement;
    private VisualElement _recipeBookHardmodeElement;
    
    private VisualElement _currentOrderContainerElement;
    private Label _currentOrderLabel;
    private VisualElement _currentPotionIconElement;
    
    private VisualElement _timerContainerElement;
    private Label _timerLabel;
    
    private VisualElement _tooltipContainerElement;

    protected override void Awake()
    {
        base.Awake();
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null) return;
        
        _rootElement = _uiDocument.rootVisualElement;
        _recipeBookContainerElement = _rootElement.Q<VisualElement>("recipeBookContainer");
        _recipeBookNormalElement = _recipeBookContainerElement.Q<VisualElement>("recipeBookNormal");
        _recipeBookHardmodeElement = _recipeBookContainerElement.Q<VisualElement>("recipeBookHardmode");
        
        _currentOrderContainerElement = _rootElement.Q<VisualElement>("orderContainer");
        _currentOrderLabel = _currentOrderContainerElement.Q<Label>("MessageBox-Body");
        _currentPotionIconElement = _currentOrderContainerElement.Q<VisualElement>("potionIcon");
        
        _timerContainerElement = _rootElement.Q<VisualElement>("timerContainer");
        _timerLabel = _timerContainerElement.Q<Label>("timerLabel");
        
        _tooltipContainerElement = _rootElement.Q<VisualElement>("tooltipContainer");

        // Initially hide the recipe book and current order elements
        _recipeBookContainerElement.style.display = DisplayStyle.None;
        _currentOrderContainerElement.style.display = DisplayStyle.None;
    }

    private void Start()
    {
        EventBus<OnNewOrderEvent>.AddListener(new EventBinding<OnNewOrderEvent>(OnNewOrder));
        
        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeTimerTickEvent>.AddListener(new EventBinding<OnHardmodeTimerTickEvent>(OnHardmodeTimerTick));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
        
        EventBus<OnInteractableHoverStartEvent>.AddListener(new EventBinding<OnInteractableHoverStartEvent>(OnInteractableHoverStart));
        EventBus<OnInteractableHoverEndEvent>.AddListener(new EventBinding<OnInteractableHoverEndEvent>(OnInteractableHoverEnd));
    }
    
    private void OnDestroy()
    {
        EventBus<OnNewOrderEvent>.RemoveListener(new EventBinding<OnNewOrderEvent>(OnNewOrder));
        
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeTimerTickEvent>.RemoveListener(new EventBinding<OnHardmodeTimerTickEvent>(OnHardmodeTimerTick));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
        
        EventBus<OnInteractableHoverStartEvent>.RemoveListener(new EventBinding<OnInteractableHoverStartEvent>(OnInteractableHoverStart));
        EventBus<OnInteractableHoverEndEvent>.RemoveListener(new EventBinding<OnInteractableHoverEndEvent>(OnInteractableHoverEnd));
    }

    public void ToggleRecipeBook()
    {
        if (_recipeBookContainerElement == null) return;
        Debug.Log("Toggling recipe book.");
        EventBus<OnRecipeBookOpenedEvent>.Raise(new OnRecipeBookOpenedEvent());
        
        _recipeBookContainerElement.style.display = 
            _recipeBookContainerElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void ToggleRecipeBookMode(bool isHardmode)
    {
        if (_recipeBookNormalElement == null || _recipeBookHardmodeElement == null) return;
        
        _recipeBookNormalElement.style.display = isHardmode ? DisplayStyle.None : DisplayStyle.Flex;
        _recipeBookHardmodeElement.style.display = isHardmode ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    private void OnNewOrder(OnNewOrderEvent evt)
    {
        if (_currentOrderLabel == null || HardmodeManager.Instance.isHardmodeActive) return;
        
        _currentOrderContainerElement.style.display = DisplayStyle.Flex;
        const string baseText = "¡Oye! Necesito que hagas una poción de ";
        _currentOrderLabel.text = baseText + $"<b>{evt.potionData.name}</b>...";
        _currentPotionIconElement.style.backgroundImage = new StyleBackground(evt.potionData.icon);
    }

    private void OnHardmodeStarted()
    {
        if (_currentOrderContainerElement == null) return;
        
        _currentOrderContainerElement.style.display = DisplayStyle.None;
        ToggleRecipeBookMode(true);

        if (_timerContainerElement == null) return;
        _timerContainerElement.style.display = DisplayStyle.Flex;
    }
    
    private void OnHardmodeTimerTick(OnHardmodeTimerTickEvent evt)
    {
        if (_timerLabel == null) return;
        
        var seconds = (int)evt.timeRemaining;
        _timerLabel.text = seconds.ToString();
    }
    
    private void OnHardmodeFailed()
    {
        if (_currentOrderContainerElement == null) return;
        
        _currentOrderContainerElement.style.display = DisplayStyle.Flex;
        ToggleRecipeBookMode(false);
        
        _timerContainerElement.style.display = DisplayStyle.None;
    }
    
    private void OnInteractableHoverStart(OnInteractableHoverStartEvent evt)
    {
        if (_tooltipContainerElement == null || IngredientHandlingManager.Instance.currentlyPickedUpIngredient) return;

        _tooltipContainerElement.style.opacity = 1.5f;
        _tooltipContainerElement.Q<Label>("tooltipLabel").text = evt.tooltipText;
    }
    
    private void OnInteractableHoverEnd(OnInteractableHoverEndEvent evt)
    {
        if (_tooltipContainerElement == null) return;

        _tooltipContainerElement.style.opacity = 0;
    }
}