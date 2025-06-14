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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Attackable"), false);
        SceneManager.LoadScene("Menu");
    }


    public void Continue()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Attackable"), false);
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string lastEscene = PlayerPrefs.GetString("LastScene");
            SceneManager.LoadScene(lastEscene, LoadSceneMode.Single);
        }   
    }
}
