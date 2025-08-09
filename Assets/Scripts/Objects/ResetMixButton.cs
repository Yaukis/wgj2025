public class ResetMixButton : Interactable
{
    private void Awake()
    {
        tooltipText = "Reiniciar poción";
    }
    
    private void OnMouseDown()
    {
        // Reset the mix when the button is clicked
        MixingManager.Instance.ClearMix();
    }
}