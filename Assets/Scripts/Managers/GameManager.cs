using UnityEngine;
using Utils;
using Utils.EventBus;

public class GameManager : MonoSingleton<GameManager>
{
    public bool isGameOver;

    private void Start()
    {
        OrderManager.Instance.GenerateNewOrder();
        
        EventBus<OnOrderCompletedEvent>.AddListener(new EventBinding<OnOrderCompletedEvent>(OnOrderCompleted));
    }
    
    private void OnDestroy()
    {
        EventBus<OnOrderCompletedEvent>.RemoveListener(new EventBinding<OnOrderCompletedEvent>(OnOrderCompleted));
    }
    
    private void OnOrderCompleted()
    {
        Debug.Log($"Order completed.");
        if (!HardmodeManager.Instance.isHardmodeActive)
        {
            HardmodeManager.Instance.SetHardmode(true);
        }
        else
        {
            isGameOver = true;
            Debug.Log("Game Over! You win.");
            EventBus<OnGameOverEvent>.Raise(new OnGameOverEvent());
            Invoke(nameof(PauseGame), 2f);
        }
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
}