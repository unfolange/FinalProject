using UnityEngine;
using UnityEngine.SceneManagement;  // 1️⃣ Importa el namespace de escenas

public class GameOverMenu : MonoBehaviour
{
    // 2️⃣ Método público que cargará la escena Main
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");  // Puedes usar el nombre de la escena
        // SceneManager.LoadScene(0);     // O el índice (ej. 0) si prefieres
    }
}
