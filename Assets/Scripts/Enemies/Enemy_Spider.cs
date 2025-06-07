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

                if (Mathf.Abs(directionToPlayer) > 0.1f)
                {
                    // Cambiar dirección de la araña según posición del jugador
                    transform.localScale = new Vector3(Mathf.Sign(directionToPlayer), transform.localScale.y, transform.localScale.z);

                    rb.linearVelocity = new Vector2(Mathf.Sign(directionToPlayer) * speed, rb.linearVelocity.y);
                }

                // Si el jugador se aleja, volver a patrullar
                if (Vector2.Distance(transform.position, player.transform.position) > detectionRange)
                {
                    ChangeState(EnemyStates.Spider_Walk);
                }

                if (Vector2.Distance(transform.position, player.transform.position) <= acidAttackRange)
                {
                    //TryShootAcid();
                    //ThrowAcidAtPlayer();
                }

                break;
        }

    }

    /// <summary>
    /// Recibir daño
    /// </summary>
    public override void TakeDamage(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.TakeDamage(_damageDone, _hitDirection, _hitForce);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 ledgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + ledgeCheckStartPoint, transform.position + ledgeCheckStartPoint + Vector3.down * ledgeCheckY);
    }



    private void TryShootAcid()
    {
        if (Time.time < lastAcidTime + acidCooldown) return;

        Vector2 targetPos = player.transform.position;

        // Introduce error en la puntería
        targetPos += new Vector2(Random.Range(-aimError, aimError), Random.Range(-aimError, aimError));

        Vector2 startPos = firePoint.position;
        Vector2 toTarget = targetPos - startPos;

        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float angleDegrees = 45f; // Ángulo fijo razonable para parábola
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        float distance = toTarget.magnitude;
        float heightDifference = toTarget.y;

        // Asegura que no se divide por 0 ni se saca raíz negativa
        float underRoot = Mathf.Pow(acidForce, 2) - gravity * (gravity * Mathf.Pow(distance, 2) + 2 * heightDifference * Mathf.Pow(acidForce, 2));
        if (underRoot <= 0)
        {
            Debug.LogWarning("No se puede disparar: objetivo fuera de alcance o demasiado cerca.");
            return;
        }

        // Dirección horizontal normalizada
        Vector2 direction = toTarget.normalized;

        // Aplica rotación para ángulo
        Vector2 launchDirection = Quaternion.Euler(0, 0, angleDegrees) * direction;

        GameObject acid = Instantiate(acidPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbAcid = acid.GetComponent<Rigidbody2D>();
        rbAcid.linearVelocity = launchDirection * acidForce;

        lastAcidTime = Time.time;
    }


    public void ThrowAcidAtPlayer()
    {
        if (player == null || acidPrefab == null || firePoint == null)
            return;

        Vector2 targetPos = player.transform.position;
        Vector2 startPos = firePoint.position;
        Vector2 direction = targetPos - startPos;

        // Margen de error: ajusta la posición del jugador con un pequeño offset aleatorio
        float inaccuracy = 0.5f; // en unidades del mundo
        direction.x += Random.Range(-inaccuracy, inaccuracy);
        direction.y += Random.Range(-inaccuracy, inaccuracy);

        // Ajusta este valor para cambiar la curva (más alto = más curva)
        float angleDegrees = 45f;

        // Calculamos la distancia horizontal y vertical
        float distanceX = direction.x;
        float distanceY = direction.y;

        // Convertimos ángulo a radianes
        float angleRad = angleDegrees * Mathf.Deg2Rad;

        // Asegura que no se divide entre cero
        float cosAngle = Mathf.Cos(angleRad);
        float sinAngle = Mathf.Sin(angleRad);

        if (Mathf.Abs(cosAngle) < 0.01f)
        {
            Debug.LogWarning("El ángulo es muy vertical, ajusta angleDegrees");
            return;
        }

        // Fórmula física para proyectil lanzado con ángulo
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float numerator = gravity * Mathf.Pow(distanceX, 2);
        float denominator = 2 * (distanceY - Mathf.Tan(angleRad) * distanceX) * Mathf.Pow(cosAngle, 2);

        if (denominator <= 0)
        {
            Debug.LogWarning("No se puede calcular una trayectoria válida (denominator <= 0)");
            return;
        }

        float velocity = Mathf.Sqrt(numerator / denominator);

        if (float.IsNaN(velocity))
        {
            Debug.LogWarning("Velocidad resultó en NaN. Revisa la distancia y el ángulo.");
            return;
        }

        // Instanciamos el proyectil
        GameObject projectile = Instantiate(acidPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Direcciones X y Y según ángulo
            Vector2 launchVelocity = new Vector2(
                velocity * cosAngle * Mathf.Sign(distanceX),
                velocity * sinAngle
            );

            rb.linearVelocity = launchVelocity;
        }
    }



}
