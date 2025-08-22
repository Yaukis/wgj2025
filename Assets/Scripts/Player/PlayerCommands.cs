using UnityEngine;
using Utils.EventBus;

public class PlayerCommands : MonoBehaviour
{
    private const bool DebugCommandsEnabled = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HUDManager.Instance.ToggleRecipeBook(false);
        }
        
        HandleDebugCommands();
    }
    
    private void HandleDebugCommands()
    {
        if (!DebugCommandsEnabled) return;
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            HUDManager.Instance.ToggleRecipeBookMode(true);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HardmodeManager.Instance.SetHardmode(!HardmodeManager.Instance.isHardmodeActive);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            EventBus<OnOrderCompletedEvent>.Raise(new OnOrderCompletedEvent());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            EventBus<OnOrderFailedEvent>.Raise(new OnOrderFailedEvent());
        }
    }
}