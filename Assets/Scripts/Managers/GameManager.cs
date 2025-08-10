using UnityEngine;
using Utils;
using Utils.EventBus;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject playerGameObject;
    
    public bool isGameRunning;

    private void Start()
    {
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
            isGameRunning = false;
            Debug.Log("Game Over! You win.");
            EventBus<OnGameOverEvent>.Raise(new OnGameOverEvent());
            Invoke(nameof(PauseGame), 2f);
        }
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        EventBus<OnGameStartEvent>.Raise(new OnGameStartEvent());
        menuCamera.SetActive(false);
        playerGameObject.SetActive(true);
    }
}