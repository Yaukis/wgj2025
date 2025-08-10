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

    private void Start()
    {
        ToggleIngredientModel(HardmodeManager.Instance.isHardmodeActive);
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
    
    private void ToggleIngredientModel(bool isHardmode)
    {
        if (isHardmode)
        {
            if (normalIngredientModel) normalIngredientModel.SetActive(false);
            if (hardmodeIngredientModel) hardmodeIngredientModel.SetActive(true);
            tooltipText = hardmodeIngredientData.name; // Update tooltip text to hardmode ingredient name
        }
        else
        {
            if (normalIngredientModel) normalIngredientModel.SetActive(true);
            if (hardmodeIngredientModel) hardmodeIngredientModel.SetActive(false);
            tooltipText = normalIngredientData.name; // Reset tooltip text to normal ingredient name
        }
    }
}