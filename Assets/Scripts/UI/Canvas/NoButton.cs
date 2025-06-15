using UnityEngine;

public class NoButton : MonoBehaviour
{
    public GameObject desGame;
    public GameObject mainMenu;
    public GameObject continuePanel;

    public void DesactiveGame()
    {
        // Mostrar el nuevo panel
        desGame.SetActive(false);

    }

    public void DesContinue()
    {
        continuePanel.SetActive(false);
    }

    public void ActiveMenu()
    {
        mainMenu.SetActive(true);
    }
}