using UnityEngine;
using Utils.EventBus;

public class WorldIngredient : Interactable
{
    [SerializeField] private IngredientData normalIngredientData;
    [SerializeField] private IngredientData hardmodeIngredientData;

    [SerializeField] private GameObject normalIngredientModel;
    [SerializeField] private GameObject hardmodeIngredientModel;

    protected void Awake()
    {
        base.Awake();
        
        if (normalIngredientData == null || hardmodeIngredientData == null)
        {
            Debug.LogError("WorldIngredient: Ingredient data not assigned for " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        tooltipText = normalIngredientData.name;
    }

    private void Start()
    {
        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }
    
    private void OnDestroy()
    {
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }

    private void OnMouseDown()
    {
        if (!isActive) return;
        
        Debug.Log("WorldIngredient clicked: " + gameObject.name);
        EventBus<OnIngredientPickupEvent>.Raise(new OnIngredientPickupEvent(!HardmodeManager.Instance.isHardmodeActive
            ? normalIngredientData
            : hardmodeIngredientData));
    }
    
    private void OnMouseEnter()
    {
        if (!isActive) return;
        
        EventBus<OnInteractableHoverStartEvent>.Raise(new OnInteractableHoverStartEvent(tooltipText, true));
    }
    
    private void OnHardmodeStarted()
    {
        if (normalIngredientModel) normalIngredientModel.SetActive(false);
        if (hardmodeIngredientModel) hardmodeIngredientModel.SetActive(true);
        tooltipText = hardmodeIngredientData.name; // Update tooltip text to hardmode ingredient name
    }
    
    private void OnHardmodeFailed()
    {
        if (normalIngredientModel) normalIngredientModel.SetActive(true);
        if (hardmodeIngredientModel) hardmodeIngredientModel.SetActive(false);
        tooltipText = normalIngredientData.name; // Reset tooltip text to normal ingredient name
    }
}