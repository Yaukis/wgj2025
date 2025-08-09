using UnityEngine;
using Utils;
using Utils.EventBus;

public class GameManager : MonoSingleton<GameManager>
{
    public bool isHardmode;

    private void Start()
    {
        OrderManager.Instance.GenerateNewOrder();
        
        EventBus<OnOrderCompletedEvent>.AddListener(new EventBinding<OnOrderCompletedEvent>(OnOrderCompleted));
    }
    
    private void OnDestroy()
    {
        EventBus<OnOrderCompletedEvent>.RemoveListener(new EventBinding<OnOrderCompletedEvent>(OnOrderCompleted));
    }
    
    private void OnOrderCompleted(OnOrderCompletedEvent evt)
    {
        Debug.Log($"Order completed.");
        isHardmode = true;
        EventBus<OnHardmodeStartedEvent>.Raise(new OnHardmodeStartedEvent());
    }
}