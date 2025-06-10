using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneOnTouch2D : MonoBehaviour
{

    public string triggeringTag = "Player";
    public string sceneName;


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("get to end", other);
        if (other.gameObject.CompareTag(triggeringTag))
        {
            Debug.Log("is player", other);
            SceneManager.LoadScene(sceneName);
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