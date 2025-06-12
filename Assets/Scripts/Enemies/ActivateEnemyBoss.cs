using UnityEngine;

public class ActivateEnemyBoss : MonoBehaviour
{
    public GameObject bossFinal;

    private void Start()
    {
        bossFinal.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bossFinal.SetActive(true);
        }
    }
}
