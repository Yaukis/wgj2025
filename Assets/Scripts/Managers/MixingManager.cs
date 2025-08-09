using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class MixingManager : MonoSingleton<MixingManager>
{
    [SerializeField] private int maxIngredients = 3;
    [SerializeField] private int maxHardmodeIngredients = 4;

    private readonly List<IngredientData> _currentIngredients = new();

    private void Start()
    {
        EventBus<OnIngredientAddedToMixEvent>.AddListener(
            new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(ClearMix));
    }

    private void OnDestroy()
    {
        EventBus<OnIngredientAddedToMixEvent>.RemoveListener(
            new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(ClearMix));
    }

    private void OnIngredientAddedToMix(OnIngredientAddedToMixEvent evt)
    {
        _currentIngredients.Add(evt.ingredientData);
        Debug.Log($"Ingredient {evt.ingredientData.name} added to mix. Total ingredients: {_currentIngredients.Count}");
        
        var neededIngredients = HardmodeManager.Instance.isHardmodeActive
            ? maxHardmodeIngredients
            : maxIngredients;

        if (_currentIngredients.Count < neededIngredients) return;

        OrderManager.Instance.VerifyOrder(_currentIngredients);
        ClearMix();
    }

    public void ClearMix()
    {
        _currentIngredients.Clear();
        Debug.Log("Mix cleared.");
    }
}