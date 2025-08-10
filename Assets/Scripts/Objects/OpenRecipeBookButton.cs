public class OpenRecipeBookButton : Interactable
{
    private void Awake()
    {
        tooltipText = "Abrir recetario";
    }

    private void OnMouseDown()
    {
        if (!isActive) return;
        // Open the recipe book when the button is clicked
        HUDManager.Instance.ToggleRecipeBook();
    }
}