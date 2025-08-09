using UnityEngine;
using Utils;
using Utils.EventBus;

public class IngredientHandlingManager : MonoSingleton<IngredientHandlingManager>
{
    [SerializeField] private Camera playerCamera;
    
    private IngredientData _currentlyPickedUpIngredient;
    
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
            if (!_currentlyPickedUpIngredient) return;
            
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 5f))
            {
                if (hit.collider.CompareTag("Recipient"))
                {
                    Debug.Log($"Adding ingredient {_currentlyPickedUpIngredient.name} to mix in recipient: {hit.collider.name}");
                    EventBus<OnIngredientAddedToMixEvent>.Raise(new OnIngredientAddedToMixEvent(_currentlyPickedUpIngredient));
                }
            }
            Debug.Log("Dropping ingredient: " + _currentlyPickedUpIngredient.name);
            EventBus<OnIngredientDropEvent>.Raise(new OnIngredientDropEvent(_currentlyPickedUpIngredient));
            _currentlyPickedUpIngredient = null;
        }
    }
    
    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        _currentlyPickedUpIngredient = evt.ingredientData;
        Debug.Log("Picked up ingredient: " + _currentlyPickedUpIngredient.name);
    }
}