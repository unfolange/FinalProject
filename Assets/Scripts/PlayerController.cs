using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    public FloatingJoystick joystick;
    private bool dashButtonPressed = false;
    private bool attackButtonPressed = false;
    public bool jumpButtonPressed;
    public bool jumpButtonReleased;

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 1;
    [Space(5)]

    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45;
    private float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;
    [Space(5)]

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldDown;
    [SerializeField] private GameObject dashEffect;
    [Space(5)]


    [Header("Attack")]
    bool attack = false;
    float timeBetweenAttack, timeSinceAttack;
    [SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;
    [SerializeField] GameObject slashEffect;
    [Space(5)]

    [Header("Recoil")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;
    [Space(5)]

    PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis, yAxis;
    private float gravity;
    Animator anim;
    private bool canDash = true;
    private bool dashed;


    //Evitar players duplicados
    public static PlayerController Instance;

    public void Awake()
    {
        if (Instance != null && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }



    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        if (pState.dashing) return;
        Flip();
        Move();
        Jump();
        StartDash();
        Attack();
        Recoil();
    }

    /// <summary>
    /// Método para obtener las entradas del usuario
    /// </summary>
    void GetInputs()
    {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.K);
#else
        xAxis = joystick.Horizontal;
        yAxis = joystick.Vertical;
        attack = attackButtonPressed;
#endif
    }

    void Flip()
    {
        if (xAxis < 0 && Time.timeScale != 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            pState.lookingRight = false;
            //transform.eulerAngles = new Vector2(0, 180);
        }
        else if (xAxis > 0 && Time.timeScale != 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            pState.lookingRight = true;
            //transform.eulerAngles = new Vector2(0, 0);
        }
    }


    /// <summary>
    /// Método para mover el player
    /// </summary>
    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        bool dashInput = false;

#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        dashInput = Input.GetButtonDown("Dash");
#else
        dashInput = dashButtonPressed;
#endif


        if (dashInput && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded())
        {
            dashed = false;
        }

        // Reiniciar el botón después de procesarlo (solo en Android)
#if !UNITY_STANDALONE && !UNITY_WEBGL || UNITY_EDITOR
        dashButtonPressed = false;
#endif

    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldDown);
        canDash = true;
    }


    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack && Time.timeScale != 0)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");

            if (yAxis > 0.5f) // Attack Up
            {
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, 80, upAttackTransform);
            }
            else if (yAxis < -0.5 && !Grounded()) //Attack Down
            {
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, -90, downAttackTransform);
            }
            else //Attack left and right
            {
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, recoilXSpeed);
                SlashEffectAngle(slashEffect, 0, sideAttackTransform);
            }
        }

#if !UNITY_STANDALONE && !UNITY_WEBGL
        attackButtonPressed = false;
        jumpButtonPressed = false;
#endif

    }


    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }


        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage,
                    (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
        }
    }

    void SlashEffectAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(_attackTransform.localScale.x, _attackTransform.localScale.y);
    }


    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
        }
        else
        {
            rb.gravityScale = gravity;
        }

        //Stop recoil X
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }

        //Stop recoil Y
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        //si ya est� en el suelo deja de retroceder
        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    /// <summary>
    /// M�todo para detectar la presencia del suelo 
    /// </summary>
    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position - new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Método para que el player salte
    /// </summary>
    void Jump()
    {
        // Intentar salto en el suelo
        if ((jumpBufferCounter > 0 && coyoteTimeCounter > 0) && !pState.jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            pState.jumping = true;
        }
        //Intentar salto en el aire (doble salto)
        else if (!Grounded() && airJumpCounter < maxAirJumps && (Input.GetButtonDown("Jump") || jumpButtonPressed))
        {
            airJumpCounter++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            pState.jumping = true;
        }

        // Cortar salto si se suelta el botón mientras asciende (Variar intensidad de salto)
        if ((Input.GetButtonUp("Jump") || jumpButtonReleased) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Grounded())
        {
            pState.jumping = false;
        }

        anim.SetBool("Jumping", !Grounded());

#if !UNITY_STANDALONE && !UNITY_WEBGL
        jumpButtonPressed = false;
        jumpButtonReleased = false;
#endif
    }


    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") || jumpButtonPressed)
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter = jumpBufferCounter - Time.deltaTime * 10;
        }
    }
    public void TakeDamage(int amount)
    {
        Debug.Log($"Jugador recibió daño: {amount} puntos.");
        // Aquí puedes reducir la vida real, reproducir animaciones, etc.
    }



    //Android
    public void OnDashButtonPressed()
    {
        dashButtonPressed = true;
    }

    public void OnAttackButtonPressed()
    {
        attackButtonPressed = true;
    }

    public void OnJumpButtonDown()
    {
        jumpButtonPressed = true;
    }

    public void OnJumpButtonUp()
    {
        jumpButtonReleased = true;
    }
}
