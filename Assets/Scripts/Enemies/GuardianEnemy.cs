using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float leftLimit = -3f;
    public float rightLimit = 3f;
    public float speed = 2f;

    private bool movingRight = true;

    void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invierte el sprite horizontalmente
        transform.localScale = scale;
    }
}
