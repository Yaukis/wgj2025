using UnityEngine;
using Utils.EventBus;

public class CauldronFeedback : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on CauldronFeedback.");
        }
    }

    private void Start()
    {
        EventBus<OnOrderFailedEvent>.AddListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));
    }
    
    private void OnDestroy()
    {
        EventBus<OnOrderFailedEvent>.RemoveListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));
    }
    
    private void OnOrderFailed(OnOrderFailedEvent evt)
    {
        Debug.Log("Order failed! Displaying cauldron feedback.");
        
        if (_animator != null) _animator.SetTrigger("Fail");
    }
}