using System.Collections;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("Attack Settings Boss")]
    [SerializeField] private Transform sideAttackTransform;
    [SerializeField] private float attackRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    protected override void Update()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);
        anim.SetFloat("playerDistance", playerDistance);
        LookPlayer();
    }

    private void Death()
    {
        Destroy(gameObject);
    }


    public void LookPlayer()
    {
        if (transform.position.x < player.transform.position.x && !isLookingRight)
        {
            isLookingRight = !isLookingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (player.transform.position.x < transform.position.x && isLookingRight)
        {
            isLookingRight = !isLookingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    protected override void Attack()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(sideAttackTransform.position, attackRadius);

        foreach(Collider2D collision in objects)
        {
            if (collision.gameObject.CompareTag("Player"))
            {                
                if (!collision.GetComponent<PlayerStateList>().dashing)
                {
                    collision.GetComponent<PlayerController>().TakeDamage(damage);
                }               
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(sideAttackTransform.position, attackRadius);
    }
}

