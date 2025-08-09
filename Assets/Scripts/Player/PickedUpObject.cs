using System;
using UnityEngine;
using Utils.EventBus;

public class PickedUpObject : MonoBehaviour
{
    [SerializeField] private GameObject pickedUpObjectModel;
    
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
            pickedUpObjectModel.SetActive(true);
            pickedUpObjectModel.transform.rotation = ingredientModel.transform.localRotation; // Reset rotation
            pickedUpObjectModel.transform.localScale = ingredientModel.transform.localScale; // Reset scale
            pickedUpObjectModel.GetComponent<MeshFilter>().mesh = ingredientModel.GetComponentInChildren<MeshFilter>().sharedMesh;
            pickedUpObjectModel.GetComponent<MeshRenderer>().material = ingredientModel.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        }
        else
        {
            Debug.LogWarning("Ingredient model is null for: " + evt.ingredientData.name);
            pickedUpObjectModel.SetActive(false);
        }
    }
    
    private void OnIngredientDrop(OnIngredientDropEvent evt)
    {
        pickedUpObjectModel.SetActive(false);
    }
}