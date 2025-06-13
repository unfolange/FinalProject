using System.Collections;
using UnityEngine;

public class Enemy_Spider : Enemy
{
    [Header("Acid Settings")]
    [SerializeField] private GameObject acidPrefab;
    [SerializeField] private Transform firePoint; // un punto desde donde se dispara el ácido
    [SerializeField] private float acidForce = 20f;
    [SerializeField] private float acidCooldown = 2f;
    [SerializeField] private float acidAttackRange = 7f;
    [SerializeField] private float aimError = 1.5f; // margen de error en unidades

    [SerializeField] private int numberOfProjectiles = 5;
    [SerializeField] private float projectileForce = 8f; // Ajusta según alcance
    [SerializeField] private float inaccuracy = 5f;

    private float lastAcidTime;

    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float flipWaitTime;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask whatIsGround;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
       // rb.gravityScale = 12f;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }*/

        if (player != null && currentEnemyState != EnemyStates.Spider_Chase)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                ChangeState(EnemyStates.Spider_Chase);
            }
        }

    }

    protected override void UpdateEnemyStates()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Spider_Walk:
                Vector3 _ledgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector2 _walkCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                anim.SetBool("Walking", true);

                if (transform.localScale.x > 0)
                {
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                }

                if (!Physics2D.Raycast(transform.position + _ledgeCheckStartPoint, Vector3.down, ledgeCheckY, whatIsGround)
                    || Physics2D.Raycast(transform.position, _walkCheckDir, ledgeCheckY, whatIsGround))
                {
                    ChangeState(EnemyStates.Spider_Flip);
                    anim.SetBool("Walking", false);
                }


                break;

            case EnemyStates.Spider_Flip:
                timer += Time.deltaTime;

                if (timer > flipWaitTime)
                {
                    timer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Spider_Walk);
                }

                break;


            case EnemyStates.Spider_Chase:
                anim.SetBool("Walking", true);

                float directionToPlayer = player.transform.position.x - transform.position.x;

                if (Mathf.Abs(directionToPlayer) > 0.1f && !isRecoiling) // Se golpea retrocede y continúa atacando
                {
                    // Cambiar dirección de la araña según posición del jugador
                    transform.localScale = new Vector3(Mathf.Sign(directionToPlayer) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

                    rb.linearVelocity = new Vector2(Mathf.Sign(directionToPlayer) * speed, rb.linearVelocity.y);                    
                }

                // Si el jugador se aleja, volver a patrullar
                if (Vector2.Distance(transform.position, player.transform.position) > detectionRange)
                {
                    ChangeState(EnemyStates.Spider_Walk);
                }

                if (Vector2.Distance(transform.position, player.transform.position) <= acidAttackRange)
                {
                    ThrowAcidAtPlayer();                    
                }

                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 ledgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + ledgeCheckStartPoint, transform.position + ledgeCheckStartPoint + Vector3.down * ledgeCheckY);
    }
    

    public void ThrowAcidAtPlayer()
    {
        if (player == null || acidPrefab == null || firePoint == null)
            return;

        if (Time.time < lastAcidTime + acidCooldown)
            return;

        StartCoroutine(CO_ThrowAcidAtPlayer());

        lastAcidTime = Time.time;
    }


    public IEnumerator CO_ThrowAcidAtPlayer()
    {
        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;
        float baseAngle;

        // Decide hacia qué lado disparar
        float angleSign = directionToPlayer.x >= 0 ? 1f : -1f;

        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                baseAngle = 15 + 5 * i;//ángulos de elevación del lanzamiento de los proyectiles.

                float angleVariation = Random.Range(-inaccuracy, inaccuracy);
                float angle = (baseAngle + angleVariation) * Mathf.Deg2Rad;

                // Calculamos el vector de dirección según el ángulo
                Vector2 forceDirection = new Vector2(Mathf.Cos(angle) * angleSign, Mathf.Sin(angle)).normalized;

                GameObject projectile = Instantiate(acidPrefab, firePoint.position, Quaternion.identity);
                //projectile.transform.localScale = new Vector3(2, 2, 1);

                Rigidbody2D rbAcid = projectile.GetComponent<Rigidbody2D>();

                if (rbAcid != null)
                {
                    rbAcid.AddForce(forceDirection * projectileForce, ForceMode2D.Impulse);
                }

                Destroy(projectile, 2f);
            }

            yield return new WaitForSeconds(0.3f); //tiempo de espera entre ráfagas
        }


        lastAcidTime = Time.time;
    }







}
