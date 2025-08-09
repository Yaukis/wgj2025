using UnityEngine;
using Utils.EventBus;

public abstract class Interactable : MonoBehaviour
{
    protected string tooltipText;

    private void Awake()
    {
        if (string.IsNullOrEmpty(tooltipText))
        {
            tooltipText = gameObject.name;
        }
    }

    private void OnMouseEnter()
    {
        EventBus<OnInteractableHoverStartEvent>.Raise(new OnInteractableHoverStartEvent(tooltipText));
    }
    
    private void OnMouseExit()
    {
        EventBus<OnInteractableHoverEndEvent>.Raise(new OnInteractableHoverEndEvent());
    }
}