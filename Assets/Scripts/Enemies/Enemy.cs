using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    protected Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] protected float speed;
    public bool isLookingRight = true;
    [SerializeField] protected float damage;
    private bool isDead = false;
    [Space(5)]

    [Header("Attack Settings")]
    public float maxHealth;
    [HideInInspector] public float health;
    [SerializeField] protected float recoilLength; //tiempo de recuperacion
    [SerializeField] protected float recoilFactor; //factor multiplicador del golpe
    [SerializeField] protected bool isRecoiling = false;
    protected float recoilTimer;
    //[Space(5)]

    

    //[Header("Enemies States Setttings")]
    protected enum EnemyStates { //Spider
        Spider_Walk,
        Spider_Chase,
        Spider_Flip
    }

    protected EnemyStates currentEnemyState;

    public PlayerController player;
    protected float playerDistance;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //player = PlayerController.Instance;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        health = maxHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateEnemyStates();

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

    protected virtual void UpdateEnemyStates() 
    {

    }

    protected void ChangeState(EnemyStates _newState)
    {
        currentEnemyState = _newState;
    }


  
    public virtual void TakeDamage(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        if (isDead) return;
        health -= _damageDone;

        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection, ForceMode2D.Impulse);
            isRecoiling = true;
        }

        if (health <= 0)
        {            
            isDead = true;
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamageBoss(float _damageDone)
    {
        if (isDead) return;
        health -= _damageDone;

        if (health <= 0)
        {
            isDead = true;
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
