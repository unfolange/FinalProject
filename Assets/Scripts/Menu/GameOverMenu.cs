using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // 2️⃣ Método público que cargará la escena Main
    void Awake()
    {
        Debug.Log("GameOverMenu awake");
    }

    public void LoadMainMenu()
    {
        Debug.Log("entro");
        SceneManager.LoadScene("Menu");
    }
}
