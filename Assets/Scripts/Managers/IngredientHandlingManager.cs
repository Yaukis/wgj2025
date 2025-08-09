using UnityEngine;
using Utils;
using Utils.EventBus;

public class IngredientHandlingManager : MonoSingleton<IngredientHandlingManager>
{
    [SerializeField] private Camera playerCamera;
    
    public IngredientData currentlyPickedUpIngredient;
    
    private void Start()
    {
        EventBus<OnIngredientPickupEvent>.AddListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
    }
    
    private void OnDestroy()
    {
        EventBus<OnIngredientPickupEvent>.RemoveListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // 0 = left mouse button
        {
            if (!currentlyPickedUpIngredient) return;
            
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 5f))
            {
                if (hit.collider.CompareTag("Recipient"))
                {
                    Debug.Log($"Adding ingredient {currentlyPickedUpIngredient.name} to mix in recipient: {hit.collider.name}");
                    EventBus<OnIngredientAddedToMixEvent>.Raise(new OnIngredientAddedToMixEvent(currentlyPickedUpIngredient));
                }
            }
            Debug.Log("Dropping ingredient: " + currentlyPickedUpIngredient.name);
            EventBus<OnIngredientDropEvent>.Raise(new OnIngredientDropEvent(currentlyPickedUpIngredient));
            currentlyPickedUpIngredient = null;
        }
    }
    
    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        currentlyPickedUpIngredient = evt.ingredientData;
        Debug.Log("Picked up ingredient: " + currentlyPickedUpIngredient.name);
    }
}