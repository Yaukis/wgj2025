using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class OrderManager : MonoSingleton<OrderManager>
{
    private PotionData _currentPotion;

    public void GenerateNewOrder()
    {
        var allPotions = Resources.LoadAll<PotionData>("Potions");
        if (allPotions.Length == 0)
        {
            Debug.LogError("No potions found in Resources/Potions.");
            return;
        }

        _currentPotion = allPotions[Random.Range(0, allPotions.Length)];
        Debug.Log($"New order generated: {_currentPotion.name}");
        EventBus<OnNewOrderEvent>.Raise(new OnNewOrderEvent(_currentPotion));
    }

    public void VerifyOrder(List<IngredientData> ingredients)
    {
        if (_currentPotion == null)
        {
            Debug.LogError("No current potion set for order verification.");
            return;
        }

        if (ingredients.Count != _currentPotion.ingredients.Count)
        {
            Debug.LogError(
                $"Incorrect number of ingredients. Expected {_currentPotion.ingredients.Count}, got {ingredients.Count}.");
            return;
        }

        for (var i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i] == _currentPotion.ingredients[i]) continue;

            Debug.Log(
                $"Ingredient mismatch at index {i}. Expected {_currentPotion.ingredients[i].name}, got {ingredients[i].name}.");
            EventBus<OnOrderFailedEvent>.Raise(new OnOrderFailedEvent());
            GenerateNewOrder();
            return;
        }

        Debug.Log("Order verified successfully!");
        EventBus<OnOrderCompletedEvent>.Raise(new OnOrderCompletedEvent());
    }
}