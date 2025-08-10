using UnityEngine.Rendering;
using Utils;
using Utils.EventBus;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Volume _postProcessingVolume;
    
    private void Start()
    {
        _postProcessingVolume = gameObject.GetComponent<Volume>();
        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }
    
    private void OnDestroy()
    {
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }
    
    private void OnHardmodeStarted(OnHardmodeStartedEvent evt)
    {
        _postProcessingVolume.enabled = false;
    }

    private void OnHardmodeFailed(OnHardmodeFailedEvent evt)
    { 
        _postProcessingVolume.enabled = true;
    }
}
