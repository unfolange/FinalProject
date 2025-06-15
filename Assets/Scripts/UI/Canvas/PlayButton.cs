using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject newGamePanelDialog;
    public GameObject mainMenuContainer;
    public GameObject continuePanel;

    public void ShowNewGamePanel()
    {
        // Mostrar el nuevo panel
        newGamePanelDialog.SetActive(true);

        // Ocultar el menú principal
        mainMenuContainer.SetActive(false);
    }

    public void ContinuePanel()
    {
        continuePanel.SetActive(true);

        mainMenuContainer.SetActive(false);
    }
}
