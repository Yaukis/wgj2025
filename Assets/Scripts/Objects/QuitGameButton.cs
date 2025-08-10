using UnityEngine;

public class QuitGameButton : Interactable
{
    private void Awake()
    {
        tooltipText = "Salir del juego";
    }
    
    private void OnMouseDown()
    {
        // Quit the game when the button is clicked
        Application.Quit();
    }
}