using UnityEngine;
using Utils.EventBus;

public class WorldIngredient : MonoBehaviour
{
    [SerializeField] private IngredientData normalIngredientData;
    [SerializeField] private IngredientData hardmodeIngredientData;
    
    private void OnMouseDown()
    {
        Debug.Log("WorldIngredient clicked: " + gameObject.name);
        EventBus<OnIngredientPickupEvent>.Raise(new OnIngredientPickupEvent(!GameManager.Instance.IsHardmode ? normalIngredientData : hardmodeIngredientData));
    }
}