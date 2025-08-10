using UnityEngine;
using Utils;
using Utils.EventBus;

public class HardmodeManager : MonoSingleton<HardmodeManager>
{
    [SerializeField] private float maxTime = 30f; // Maximum time in seconds for hardmode

    public bool isHardmodeActive;
    
    private float _currentTime;
    private bool _isTimerRunning;

    private void Start()
    {
        _currentTime = maxTime;
        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
    }
    
    private void OnDestroy()
    {
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
    }
    
    private void Update()
    {
        if (!isHardmodeActive || !_isTimerRunning || !GameManager.Instance.isGameRunning) return;
        
        if (_currentTime <= 0f)
        {
            SetHardmode(false);
        }
        else
        {
            EventBus<OnHardmodeTimerTickEvent>.Raise(new OnHardmodeTimerTickEvent(_currentTime));
        }
        _currentTime -= Time.deltaTime;
    }
    
    private void OnHardmodeStarted(OnHardmodeStartedEvent evt)
    {
        _currentTime = maxTime;
        Debug.Log("Hardmode started. Time reset to " + maxTime + " seconds.");
        EventBus<OnHardmodeTimerTickEvent>.Raise(new OnHardmodeTimerTickEvent(_currentTime));
        
        // Set the timer to running state after 1 second delay
        Invoke(nameof(StartTimer), 1f);
    }
    
    private void StartTimer()
    {
        _isTimerRunning = true;
        Debug.Log("Hardmode timer started.");
    }
    
    public void SetHardmode(bool isHardmode)
    {
        isHardmodeActive = isHardmode;
        if (isHardmode)
        {
            EventBus<OnHardmodeStartedEvent>.Raise(new OnHardmodeStartedEvent());
        }
        else
        {
            EventBus<OnHardmodeFailedEvent>.Raise(new OnHardmodeFailedEvent());
            _isTimerRunning = false;
        }
    }
}