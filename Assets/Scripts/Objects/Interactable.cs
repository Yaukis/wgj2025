using UnityEngine;
using Utils.EventBus;

public abstract class Interactable : MonoBehaviour
{
    protected string tooltipText;

    protected bool isActive;

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(tooltipText))
        {
            tooltipText = gameObject.name;
        }
        
        EventBus<OnGameStartEvent>.AddListener(new EventBinding<OnGameStartEvent>(Activate));
    }

    private void OnEnable()
    {
        Invoke(nameof(Initialize), 1f);
    }

    private void OnDestroy()
    {
        EventBus<OnGameStartEvent>.RemoveListener(new EventBinding<OnGameStartEvent>(Activate));
    }

    private void OnMouseEnter()
    {
        if (!isActive) return;
        EventBus<OnInteractableHoverStartEvent>.Raise(new OnInteractableHoverStartEvent(tooltipText, false));
    }
    
    private void OnMouseExit()
    {
        if (!isActive) return;
        EventBus<OnInteractableHoverEndEvent>.Raise(new OnInteractableHoverEndEvent());
    }
    
    private void Activate()
    {
        isActive = true;
    }

    private void Initialize()
    {
        if (!GameManager.Instance.isGameRunning) return;
        
        Activate();
    }
}