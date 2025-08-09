using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed. Open menu.");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.ToggleRecipeBook();
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            UIManager.Instance.ToggleRecipeBookMode(true);
        }
    }
}