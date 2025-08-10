using UnityEngine;

namespace Utils.EventBus
{
    public interface IEvent
    {
    }
    
    
    /* Ingredients */

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
    
    
    /* Orders */

    public struct OnNewOrderEvent : IEvent
    {
        public readonly PotionData potionData;
        
        public OnNewOrderEvent(PotionData potionData)
        {
            this.potionData = potionData;
        }
    }
    
    public struct OnOrderCompletedEvent : IEvent
    {
    }
    
    public struct OnOrderFailedEvent : IEvent
    {
    }
    
    
    /* Hardmode */
    public struct OnHardmodeStartedEvent : IEvent
    {
    }
    
    public struct OnHardmodeTimerTickEvent : IEvent
    {
        public readonly float timeRemaining;

        public OnHardmodeTimerTickEvent(float timeRemaining)
        {
            this.timeRemaining = timeRemaining;
        }
    }
    
    public struct OnHardmodeCompletedEvent : IEvent
    {
    }
    
    public struct OnHardmodeFailedEvent : IEvent
    {
    }
    
    
    /* Game */
    public struct OnGameOverEvent : IEvent
    {
    }
    
    public struct OnRecipeBookOpenedEvent : IEvent
    {
    }
    
    /* Tooltip */
    public struct OnInteractableHoverStartEvent : IEvent
    {
        public readonly string tooltipText;

        public OnInteractableHoverStartEvent(string tooltipText)
        {
            this.tooltipText = tooltipText;
        }
    }
    
    public struct OnInteractableHoverEndEvent : IEvent
    {
    }
}