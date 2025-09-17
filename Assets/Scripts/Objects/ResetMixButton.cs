using Utils.EventBus;

public class ResetMixButton : Interactable
{
    private void Start()
    {
        tooltipText = "Reiniciar poción";
    }
    
    private void OnMouseDown()
    {
        if (!isActive) return;
        // Reset the mix when the button is clicked
        MixingManager.Instance.ClearMix();
        EventBus<OnPotionResetEvent>.Raise(new OnPotionResetEvent());
    }
}