public class ResetMixButton : Interactable
{
    private void Awake()
    {
        tooltipText = "Reiniciar poción";
    }
    
    private void OnMouseDown()
    {
        if (!isActive) return;
        // Reset the mix when the button is clicked
        MixingManager.Instance.ClearMix();
    }
}