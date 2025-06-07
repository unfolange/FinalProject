using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();          

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
