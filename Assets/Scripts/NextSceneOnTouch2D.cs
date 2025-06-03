using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneOnTouch2D : MonoBehaviour
{
    
    public string triggeringTag = "Player";

    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag(triggeringTag))
        {
            Debug.Log(other.gameObject.name + " ha entrado en el trigger. Cargando siguiente escena.");

            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

           
            int nextSceneIndex = currentSceneIndex + 1;

            
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    
    }

    
    void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f); // Verde semi-transparente
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        }
    }
}