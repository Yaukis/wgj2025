using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Utils.EventBus;

public class HUDManager : MonoSingleton<HUDManager>
{
    [SerializeField] private Sprite reticleIcon;
    [SerializeField] private Sprite openHandIcon;
    [SerializeField] private Sprite grabbingHandIcon;
    [SerializeField] private GameObject clickBlocker;

    private UIDocument _uiDocument;

    private VisualElement _rootElement;

    private VisualElement _recipeBookContainerElement;
    private VisualElement _recipeBookNormalElement;
    private VisualElement _recipeBookHardmodeElement;

    private VisualElement _currentOrderContainerElement;
    private VisualElement _currentOrderTextGroupElement;
    private Label _currentOrderLabel;
    private VisualElement _currentPotionIconElement;

    private VisualElement _timerContainerElement;
    private Label _timerLabel;

    private VisualElement _tooltipContainerElement;

    private VisualElement _reticleVisualElement;

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
        _currentOrderTextGroupElement = _currentOrderContainerElement.Q<VisualElement>("MessageBox-TextGroup");
        _currentOrderLabel = _currentOrderContainerElement.Q<Label>("MessageBox-Body");
        _currentPotionIconElement = _currentOrderContainerElement.Q<VisualElement>("potionIcon");

        _timerContainerElement = _rootElement.Q<VisualElement>("timerContainer");
        _timerLabel = _timerContainerElement.Q<Label>("timerLabel");

        _tooltipContainerElement = _rootElement.Q<VisualElement>("tooltipContainer");

        _reticleVisualElement = _rootElement.Q<VisualElement>("reticle");

        // Initially hide the recipe book and current order elements
        _recipeBookContainerElement.style.display = DisplayStyle.None;
    }

    private void Start()
    {
        EventBus<OnNewOrderEvent>.AddListener(new EventBinding<OnNewOrderEvent>(OnNewOrder));
        EventBus<OnOrderFailedEvent>.AddListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));

        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeTimerTickEvent>.AddListener(new EventBinding<OnHardmodeTimerTickEvent>(OnHardmodeTimerTick));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));

        EventBus<OnInteractableHoverStartEvent>.AddListener(
            new EventBinding<OnInteractableHoverStartEvent>(OnInteractableHoverStart));
        EventBus<OnInteractableHoverEndEvent>.AddListener(
            new EventBinding<OnInteractableHoverEndEvent>(OnInteractableHoverEnd));

        EventBus<OnIngredientPickupEvent>.AddListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnIngredientDropEvent>.AddListener(new EventBinding<OnIngredientDropEvent>(OnIngredientDrop));
    }

    private void OnDestroy()
    {
        EventBus<OnNewOrderEvent>.RemoveListener(new EventBinding<OnNewOrderEvent>(OnNewOrder));
        EventBus<OnOrderFailedEvent>.RemoveListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));

        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeTimerTickEvent>.RemoveListener(
            new EventBinding<OnHardmodeTimerTickEvent>(OnHardmodeTimerTick));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));

        EventBus<OnInteractableHoverStartEvent>.RemoveListener(
            new EventBinding<OnInteractableHoverStartEvent>(OnInteractableHoverStart));
        EventBus<OnInteractableHoverEndEvent>.RemoveListener(
            new EventBinding<OnInteractableHoverEndEvent>(OnInteractableHoverEnd));

        EventBus<OnIngredientPickupEvent>.RemoveListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnIngredientDropEvent>.RemoveListener(new EventBinding<OnIngredientDropEvent>(OnIngredientDrop));
    }

    public void ToggleRecipeBook()
    {
        if (_recipeBookContainerElement == null) return;
        Debug.Log("Toggling recipe book.");
        EventBus<OnRecipeBookOpenedEvent>.Raise(new OnRecipeBookOpenedEvent());

        var isRecipeBookOpen = _recipeBookContainerElement.style.display != DisplayStyle.None;
        clickBlocker.SetActive(!isRecipeBookOpen);
        _recipeBookContainerElement.style.display = !isRecipeBookOpen ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ToggleRecipeBookMode(bool isHardmode)
    {
        if (_recipeBookNormalElement == null || _recipeBookHardmodeElement == null) return;

        _recipeBookNormalElement.style.display = isHardmode ? DisplayStyle.None : DisplayStyle.Flex;
        _recipeBookHardmodeElement.style.display = isHardmode ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void ChangeOrderContainerVisibility(bool isVisible)
    {
        if (_currentOrderContainerElement == null) return;

        if (isVisible)
        {
            if (_currentOrderContainerElement.ClassListContains("visible"))
                return; // Already visible, no need to change
            
            _currentOrderContainerElement.style.translate = new StyleTranslate(new Translate(0, 0));
            _currentOrderTextGroupElement.style.opacity = 1;
            _currentOrderContainerElement.AddToClassList("visible");
        }
        else
        {
            _currentOrderContainerElement.style.translate = new StyleTranslate(new Translate(0, 400));
            _currentOrderTextGroupElement.style.opacity = 0;
            _currentOrderContainerElement.RemoveFromClassList("visible");
        }
    }

    private void OnNewOrder(OnNewOrderEvent evt)
    {
        Debug.Log(
            $"Trying to show new order UI for potion: {_currentOrderLabel}, {HardmodeManager.Instance.isHardmodeActive}");
        if (_currentOrderLabel == null || HardmodeManager.Instance.isHardmodeActive) return;
        ChangeOrderContainerVisibility(true);
        const string baseText = "¡Oye! Necesito que hagas una poción de ";
        _currentOrderLabel.text = baseText + $"<b>{evt.potionData.name}</b>...";
        _currentPotionIconElement.style.backgroundImage = new StyleBackground(evt.potionData.icon);
    }

    private void OnOrderFailed(OnOrderFailedEvent evt)
    {
        StartCoroutine(ShakeVisualElement(_currentOrderContainerElement, 10f, 0.4f));
    }

    private IEnumerator ShakeVisualElement(VisualElement element, float magnitude, float duration)
    {
        element.style.translate = new StyleTranslate(new Translate(-magnitude, 0));
        yield return new WaitForSeconds(duration / 2);
        element.style.translate = new StyleTranslate(new Translate(magnitude, 0));
        yield return new WaitForSeconds(duration / 2);
        element.style.translate = new StyleTranslate(new Translate(0, 0));
    }

    private void OnHardmodeStarted()
    {
        ChangeOrderContainerVisibility(false);
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
        ChangeOrderContainerVisibility(true);
        ToggleRecipeBookMode(false);

        if (_timerContainerElement == null) return;
        _timerContainerElement.style.display = DisplayStyle.None;
    }

    private void OnInteractableHoverStart(OnInteractableHoverStartEvent evt)
    {
        if (_tooltipContainerElement == null || IngredientHandlingManager.Instance.currentlyPickedUpIngredient) return;

        _tooltipContainerElement.style.opacity = 1.5f;
        _tooltipContainerElement.Q<Label>("tooltipLabel").text = evt.tooltipText;

        if (!evt.showHandIcon || _reticleVisualElement == null) return;

        _reticleVisualElement.style.backgroundImage = new StyleBackground(openHandIcon);
    }

    private void OnInteractableHoverEnd(OnInteractableHoverEndEvent evt)
    {
        if (_tooltipContainerElement == null) return;

        _tooltipContainerElement.style.opacity = 0;

        if (_reticleVisualElement == null || IngredientHandlingManager.Instance.currentlyPickedUpIngredient) return;

        _reticleVisualElement.style.backgroundImage = new StyleBackground(reticleIcon);
    }

    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        if (_reticleVisualElement == null) return;

        _reticleVisualElement.style.backgroundImage = new StyleBackground(grabbingHandIcon);
    }

    private void OnIngredientDrop(OnIngredientDropEvent evt)
    {
        if (_reticleVisualElement == null) return;

        _reticleVisualElement.style.backgroundImage = new StyleBackground(reticleIcon);
    }
}