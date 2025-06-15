using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Cambia esto por el nombre exacto de tu escena principal
    public string firstScene = "Level_01_land";

    public void LoadMainScene()
    {
        SceneManager.LoadScene(firstScene);
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
