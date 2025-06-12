using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;

    public System.Action OnHitPlayer;

    [SerializeField] private float pointDamage = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = transform.right * speed;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log("Collided with: " + collision.gameObject.name);

    //     if (collision.CompareTag("Player"))
    //     {
    //         OnHitPlayer?.Invoke(); // Evento para reducir la vida
    //         Destroy(gameObject);
    //     }
    //     else if (collision.CompareTag("Wall"))
    //     {
    //         Destroy(gameObject);
    //     }
    //     else return;

    // }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Projectile colisionó con: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // Debug.Log("¡Detectado Player! ¿OnHitPlayer tiene suscriptor? " + (OnHitPlayer != null));
            OnHitPlayer?.Invoke();
            Destroy(gameObject);
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                // 2) Le quitamos vida (usa tu método existente)
                player.TakeDamage(pointDamage);
            }

        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else return;

    }


}
