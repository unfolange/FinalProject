using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    protected Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    [SerializeField] protected float speed;
    public bool isLookingRight = true;
    [SerializeField] protected float damage;
    [Space(5)]

    [Header("Attack Settings")]
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    protected float recoilTimer;
    [Space(5)]
        
    public PlayerController player;
    protected float playerDistance;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //player = PlayerController.Instance;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {     
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    /// <summary>
    /// Equivalente a EnemyHit
    /// </summary>
    public virtual void TakeDamage(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        //------------------------Actualizar Barra vida

        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
            isRecoiling = true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamageBoss(float _damageDone)
    {
        health -= _damageDone;
        //------------------------Actualizar Barra vida

        if (health <= 0)
        {
            anim.SetTrigger("Death");
        }
    }

    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();           
        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(damage);
    }

}
