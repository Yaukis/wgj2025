using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class UIManager : MonoSingleton<UIManager>
{
    private UIDocument _uiDocument;
    
    private VisualElement _rootElement;
    
    private VisualElement _recipeBookContainerElement;
    private VisualElement _recipeBookNormalElement;
    private VisualElement _recipeBookHardmodeElement;
    
    private VisualElement _currentOrderContainerElement;
    private Label _currentOrderLabel;

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

        // Initially hide the recipe book and current order elements
        _recipeBookContainerElement.style.display = DisplayStyle.None;
        _currentOrderContainerElement.style.display = DisplayStyle.None;
    }

    public void ToggleRecipeBook()
    {
        if (_recipeBookContainerElement == null) return;
        Debug.Log("Toggling recipe book.");
        
        _recipeBookContainerElement.style.display = 
            _recipeBookContainerElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void ToggleRecipeBookMode(bool isHardmode)
    {
        if (_recipeBookNormalElement == null || _recipeBookHardmodeElement == null) return;
        
        _recipeBookNormalElement.style.display = isHardmode ? DisplayStyle.None : DisplayStyle.Flex;
        _recipeBookHardmodeElement.style.display = isHardmode ? DisplayStyle.Flex : DisplayStyle.None;
    }
}