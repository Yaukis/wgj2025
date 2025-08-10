using UnityEngine;

public class CloseRecipeBookButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Close the recipe book when the button is clicked
        HUDManager.Instance.ToggleRecipeBook();
    }
}