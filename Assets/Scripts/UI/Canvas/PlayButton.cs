using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject newGamePanelDialog;
    public GameObject mainMenuContainer;

    public void ShowNewGamePanel()
    {
        // Mostrar el nuevo panel
        newGamePanelDialog.SetActive(true);

        // Ocultar el menú principal
        mainMenuContainer.SetActive(false);
    }
}
