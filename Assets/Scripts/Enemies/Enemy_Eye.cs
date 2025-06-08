using UnityEngine;

public class Enemy_Eye : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;
    private float originalGravity;

    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Eye_Idle);
        originalGravity = rb.gravityScale;
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (currentEnemyState)
        {
            case EnemyStates.Eye_Idle:
                anim.SetBool("Fly", false);
                rb.gravityScale = 0f; // Flotar
                rb.linearVelocity = new Vector2(0f, 0f);               

                if (_dist< chaseDistance)
                {
                    ChangeState(EnemyStates.Eye_Chase);
                }
                break;

            case EnemyStates.Eye_Chase:
                anim.SetBool("Fly", true);
                rb.gravityScale = originalGravity;                
                //rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));

                Vector3 targetPos = PlayerController.Instance.transform.position;

                // Altura mínima deseada (puedes ajustarla)
                float minHeight = 1f;

                // Eleva el enemy si está muy abajo y ataca a más velocidad
                if (transform.position.y < targetPos.y + minHeight)
                {       
                    targetPos.y += minHeight;
                    rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed * 5));
                }
                else
                {
                    rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed));
                }

                if (_dist >= chaseDistance)
                {
                    ChangeState(EnemyStates.Eye_Idle);
                }

                FlipEye();
                break;

            case EnemyStates.Eye_Stunned:
                anim.SetBool("Stuned", true);
                rb.gravityScale = originalGravity;

                timer += Time.deltaTime;

                if (timer > stunDuration)
                {
                    anim.SetBool("Stuned", false);
                    ChangeState(EnemyStates.Eye_Chase);
                    timer = 0;
                }

                break;
        }
    }
       

    void FlipEye()
    {
        sr.flipX = PlayerController.Instance.transform.position.x < transform.position.x;       
    }

    public override void TakeDamage(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.TakeDamage(_damageDone, _hitDirection, _hitForce);

        if (health > 0)
        {
            ChangeState(EnemyStates.Eye_Stunned);
        }      
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
