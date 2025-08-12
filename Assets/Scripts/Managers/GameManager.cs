using FollowCamera;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject playerGameObject;
    
    public bool isGameRunning;

    private CameraFollow _cameraFollow;
    
    private void Start()
    {
        EventBus<OnOrderCompletedEvent>.AddListener(new EventBinding<OnOrderCompletedEvent>(OnOrderCompleted));
        
        _cameraFollow = playerGameObject.GetComponentInChildren<CameraFollow>();
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
            playerGameObject.SetActive(false);
            EventBus<OnGameOverEvent>.Raise(new OnGameOverEvent());
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        EventBus<OnGameStartEvent>.Raise(new OnGameStartEvent());
        isGameRunning = true;
        menuCamera.SetActive(false);
        playerGameObject.SetActive(true);
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        if (!_cameraFollow)
        {
            Debug.LogError("No cameraFollow found.");
            return;
        }
        
        _cameraFollow.SetMouseSensitivity(sensitivity, sensitivity);
    }
}