using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToLoad : MonoBehaviour
{
    [Tooltip("Nombre o índice de la escena a cargar")]
    public string sceneName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
