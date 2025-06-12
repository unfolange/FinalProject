using UnityEngine;

public class FireHazard : MonoBehaviour
{
    [Header("Respawn Settings")]
    [Tooltip("Arrastra aquí el Empty GameObject que marca el punto de reaparición")]
    [SerializeField] private Transform respawnPoint;

    [Header("Damage Settings")]
    [Tooltip("Cuántas unidades de vida le resta al jugador")]
    [SerializeField] private int fireDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con el jugador...
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                // 1) Lo movemos al punto de respawn
                player.transform.position = respawnPoint.position;
                // 2) Le quitamos vida (usa tu método existente)
                player.TakeDamage(fireDamage);
            }
        }
    }
}
