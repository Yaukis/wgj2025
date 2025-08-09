using UnityEngine;
using UnityEngine.Serialization;
using Utils.EventBus;

public class PickedUpObject : MonoBehaviour
{
    [FormerlySerializedAs("pickedUpObjectModel")] [SerializeField] private GameObject pickedUpObject;
    
    private GameObject _pickedUpObjectModel;
    
    private void Awake()
    {
        EventBus<OnIngredientPickupEvent>.AddListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnIngredientDropEvent>.AddListener(new EventBinding<OnIngredientDropEvent>(OnIngredientDrop));
    }
    
    private void OnDestroy()
    {
        EventBus<OnIngredientPickupEvent>.RemoveListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnIngredientDropEvent>.RemoveListener(new EventBinding<OnIngredientDropEvent>(OnIngredientDrop));
    }
    
    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        // Set the model to the picked up ingredient based on the ingredient data
        var ingredientModel = evt.ingredientData.grabPrefab;
        if (ingredientModel != null)
        {
            pickedUpObject.SetActive(true);
            _pickedUpObjectModel = Instantiate(ingredientModel, pickedUpObject.transform);
            _pickedUpObjectModel.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Ingredient model is null for: " + evt.ingredientData.name);
            pickedUpObject.SetActive(false);
        }
    }
    
    private void OnIngredientDrop(OnIngredientDropEvent evt)
    {
        pickedUpObject.SetActive(false);
        Destroy(_pickedUpObjectModel);
    }
}