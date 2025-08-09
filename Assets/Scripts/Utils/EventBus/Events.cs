using UnityEngine;

namespace Utils.EventBus
{
    public interface IEvent
    {
    }

    public struct OnIngredientPickupEvent : IEvent
    {
        public readonly IngredientData ingredientData;

        public OnIngredientPickupEvent(IngredientData ingredientData)
        {
            this.ingredientData = ingredientData;
        }
    }
    
    public struct OnIngredientDropEvent : IEvent
    {
        public readonly IngredientData ingredientData;

        public OnIngredientDropEvent(IngredientData ingredientData)
        {
            this.ingredientData = ingredientData;
        }
    }
    
    public struct OnIngredientAddedToMixEvent : IEvent
    {
        public readonly IngredientData ingredientData;

        public OnIngredientAddedToMixEvent(IngredientData ingredientData)
        {
            this.ingredientData = ingredientData;
        }
    }
}