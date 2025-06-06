using UnityEngine;

public class Enemy_BossSkill : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Vector2 boxArea;
    [SerializeField] private Transform boxTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        Collider2D[] objects = Physics2D.OverlapBoxAll(boxTransform.position, boxArea, 0);

        foreach(Collider2D collisions in objects)
        {
            if (collisions.gameObject.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage);
                //collisions.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(boxTransform.position, boxArea);
    }
}
