using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class MixingManager : MonoSingleton<MixingManager>
{
    private List<IngredientData> _currentIngredients = new();

    private void Start()
    {
        EventBus<OnIngredientAddedToMixEvent>.AddListener(new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
    }
    
    private void OnDestroy()
    {
        EventBus<OnIngredientAddedToMixEvent>.RemoveListener(new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
    }
    
    private void OnIngredientAddedToMix(OnIngredientAddedToMixEvent evt)
    {
        _currentIngredients.Add(evt.ingredientData);
        Debug.Log($"Ingredient {evt.ingredientData.name} added to mix. Total ingredients: {_currentIngredients.Count}");
    }
}