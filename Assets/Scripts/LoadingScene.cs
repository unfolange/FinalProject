using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class LoadingScene : MonoBehaviour
{
    // Esta función se llama cuando el usuario ha presionado el botón del mouse
    // mientras el cursor está sobre el Collider de este GameObject.
    // También funciona con toques en dispositivos móviles si tienes un PhysicsRaycaster en tu cámara.
    void OnMouseDown()
    {
        Debug.Log(gameObject.name + " ha sido clickeado/tocado. Cargando siguiente escena...");

        // Obtener el índice de la escena actual en la configuración de compilación
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calcular el índice de la siguiente escena
        int nextSceneIndex = currentSceneIndex + 1;

        // Verificar si la siguiente escena existe en la configuración de compilación
        // SceneManager.sceneCountInBuildSettings te da el número total de escenas en tu Build Settings.
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Cargar la siguiente escena
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Opcional: Si no hay más escenas, puedes cargar la primera escena (índice 0)
            // o mostrar un mensaje de advertencia.
            Debug.LogWarning("No hay más escenas para cargar, o la siguiente escena no está en Build Settings. Volviendo a la primera escena.");
            SceneManager.LoadScene(0); // Carga la primera escena (índice 0)
            // O podrías simplemente no hacer nada:
            // Debug.LogWarning("No hay más escenas para cargar.");
        }
    }

    // Opcional: Si quieres cargar una escena específica por nombre en lugar de la "siguiente"
    /*
    public string sceneNameToLoad; // Asigna el nombre de la escena en el Inspector

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.Log(gameObject.name + " ha sido clickeado/tocado. Cargando escena: " + sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogError("El nombre de la escena a cargar no está configurado en el Inspector.");
        }
    }
    */
}
