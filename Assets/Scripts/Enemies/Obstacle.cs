using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] float fallSpeed = 2f;

    [SerializeField] float timeOfDestroyOnGround = 0.2f;
    [SerializeField] float timeOfDestroyOnPlayer = 0.1f;
    [SerializeField] float activationDistance = 10f;
    [SerializeField] float damageOnPlayer = 1f;  // <-- daño a infligir

    Rigidbody2D rbEnemy;
    Transform playerTransform;
    bool hasBeenActivated = false;

    void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        if (rbEnemy == null)
        {
            Debug.LogError("Rigidbody2D no encontrado en " + gameObject.name + ". Por favor, añade uno.", this);
            enabled = false;
            return;
        }
        rbEnemy.bodyType = RigidbodyType2D.Kinematic;

        // Buscar el jugador por su Tag. 
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

    }

    void Update()
    {

        if (!hasBeenActivated && playerTransform != null)
        {

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= activationDistance)
            {
                rbEnemy.bodyType = RigidbodyType2D.Dynamic;
                hasBeenActivated = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            var pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
                pc.TakeDamage(damageOnPlayer);

            Destroy(gameObject, timeOfDestroyOnPlayer);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, timeOfDestroyOnGround);
        }
    }

    private void FixedUpdate()
    {

        if (rbEnemy.bodyType == RigidbodyType2D.Dynamic)
        {
            rbEnemy.AddForce(Vector2.down * fallSpeed, ForceMode2D.Force);
        }
    }
}