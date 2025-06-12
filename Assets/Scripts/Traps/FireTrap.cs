using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;   
    [SerializeField] private float damage = 1;

    private float respawnDelay = 0.1f;

    private void Awake()
    {
        if (respawnPoint == null)
            Debug.LogWarning("Point Reborn is not assigned!", this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Co_RespawnPlayer(collision.gameObject));            
        }
    }


    IEnumerator Co_RespawnPlayer(GameObject player)
    {
        player.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        
        player.transform.position = respawnPoint.position;        
        yield return new WaitForSeconds(respawnDelay);

        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }
}
