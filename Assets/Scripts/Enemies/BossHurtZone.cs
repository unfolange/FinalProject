using System.Collections;
using UnityEngine;

public class BossHurtZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float bounceForce = 8f;
    [SerializeField] private float horizontalForce = 5f;
    [SerializeField] private float verticalOffsetThreshold = 0.5f;   

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (player != null && rb != null)
            {                
                player.TakeDamage(damageAmount);

                Vector2 bossPos = transform.position;
                Vector2 playerPos = collision.transform.position;

                float yDifference = playerPos.y - bossPos.y;

                // Si el player está claramente por encima
                if (yDifference > verticalOffsetThreshold)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForceY(bounceForce, ForceMode2D.Impulse);
                }
                else
                {
                    player.isKnockback = true;
                    float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
                    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                    rb.AddForceX(dir * horizontalForce, ForceMode2D.Impulse);
                    StartCoroutine(stopKockBack(player));
                }
            }
        }
    }

    IEnumerator stopKockBack(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(1f);    
        player.isKnockback = false;
    }
}

