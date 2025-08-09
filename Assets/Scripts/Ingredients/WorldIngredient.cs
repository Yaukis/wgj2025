using UnityEngine;
using Utils.EventBus;

public class WorldIngredient : Interactable
{
    [SerializeField] private IngredientData normalIngredientData;
    [SerializeField] private IngredientData hardmodeIngredientData;

    private void Awake()
    {
        if (normalIngredientData == null || hardmodeIngredientData == null)
        {
            Debug.LogError("WorldIngredient: Ingredient data not assigned for " + gameObject.name);
        }
        
        tooltipText = normalIngredientData != null ? normalIngredientData.name : "Unknown Ingredient";
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
        Debug.Log("WorldIngredient clicked: " + gameObject.name);
        EventBus<OnIngredientPickupEvent>.Raise(new OnIngredientPickupEvent(!HardmodeManager.Instance.isHardmodeActive
            ? normalIngredientData
            : hardmodeIngredientData));
    }
    
    private void OnHardmodeStarted()
    {
        tooltipText = hardmodeIngredientData.name;
    }
    
    private void OnHardmodeFailed()
    {
        tooltipText = normalIngredientData.name;
    }
}