using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    public FloatingJoystick joystick;
    public ActionsButtons actionsButtons;
    private bool dashButtonPressed = false;
    private bool attackButtonPressed = false;
    private bool jumpButtonPressed;
    private bool jumpButtonReleased;

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
    private int maxAirJumps;
    [Space(5)]

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [Space(5)]

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] public float dashTime;
    [SerializeField] private float dashCooldDown;
    [SerializeField] private GameObject dashEffect;
    private bool canDash;
    private bool dashed;
    [Space(5)]


    [Header("Attack Settings")]
    bool attack = false;
    bool canAttack = false;
    [SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;
    [SerializeField] GameObject slashEffect;
    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttack;
    public bool isKnockBack;
   [Space(5)]

    [Header("Recoil")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;
    [Space(5)]

    [Header("Health Settings")]
    [SerializeField] GameObject bloodSpurt;
    public float maxHealth;
    [HideInInspector] public float health;    
    [SerializeField] float hitFlashSpeed;
    private SpriteRenderer spritePlayer;
    [HideInInspector] public bool isKnockback = false;
    private bool isAlive = true;
    [Space(5)]

    [HideInInspector] public PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis, yAxis;
    private float gravity;
    Animator anim;
 

    //Time
    bool restoreTime;
    float restoreTimeSpeed;

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

        health = maxHealth;        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gravity = rb.gravityScale;
        spritePlayer = GetComponent<SpriteRenderer>();

        UpdateSkills();

        //Guardar Avance
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
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
        if (isAlive)
        {
            GetInputs();
            UpdateJumpVariables();
            Jump();
            if (pState.dashing) return;
            Flip();
            Move();
            
            StartDash();
            Attack();
        }

        RestoreTimeScale();
        FlashWhileInvicible();
    }

    private void FixedUpdate()
    {
        if (pState.dashing) return;
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

        // Resetear el estado del botón inmediatamente después de usarlo
        if (jumpButtonPressed) 
        {            
            jumpBufferCounter = jumpBufferFrames; // Simular el input down
        }

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
        if (!isKnockback)
        {
            rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocity.y);
            anim.SetBool("Walking", rb.linearVelocity.x != 0 && Grounded());
        }
    }

    //Actualizar las habilidades desbloqueadas
    public void UpdateSkills()
    {
        maxAirJumps = pState.canDobleJump ? 2 : (pState.canJump ? 1 : 0);
        canDash = pState.canDash;
        canAttack = pState.canAttack;


#if UNITY_ANDROID

        //joystick.AxisOptions = canAttack ? AxisOptions.Both : AxisOptions.Horizontal;
        //joystick.AxisOptions = AxisOptions.Horizontal;

        actionsButtons.UnlockJump(maxAirJumps > 0);
        actionsButtons.UnlockDash(canDash);
        actionsButtons.UnlockAttack(canAttack);
#endif


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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Attackable"), true);

        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (Grounded()) Instantiate(dashEffect, transform);
        yield return new WaitForSecondsRealtime(dashTime);

        rb.gravityScale = gravity;        
        pState.dashing = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Attackable"), false);

        yield return new WaitForSecondsRealtime(dashCooldDown);
        canDash = true;
    }


    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack && Time.timeScale != 0 && canAttack)
        {
            timeSinceAttack = 0;
            anim.SetTrigger("Attacking");
                        
            if (yAxis > 0.5f) // Attack Up
            {
                anim.SetInteger("attackDirection", 1);
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, 80, upAttackTransform);
            }
            else if (yAxis < -0.5 && !Grounded()) //Attack Down
            {
                anim.SetInteger("attackDirection", -1);
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);
                SlashEffectAngle(slashEffect, -90, downAttackTransform);
            }
            else //Attack left and right
            {
                anim.SetInteger("attackDirection", 0);
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
        List<Enemy> enemiesHited = new List<Enemy>();

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            Enemy enemy = objectsToHit[i].GetComponent<Enemy>();

            if (enemy != null)
            {
                if (enemy.gameObject.CompareTag("Enemy") && !enemiesHited.Contains(enemy))
                {
                    enemy.TakeDamage(damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
                    enemiesHited.Add(enemy);
                }
                else if (enemy.gameObject.CompareTag("Boss"))
                {
                    enemy.TakeDamageBoss(damage);
                }
            }
        }
    }

    void SlashEffectAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(_attackTransform.localScale.x, _attackTransform.localScale.y) * 0.75f;
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

        //si ya está en el suelo deja de retroceder
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

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }


    /// <summary>
    /// Método para detectar la presencia del suelo 
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
        if ((jumpBufferCounter > 0 && coyoteTimeCounter > 0) && !pState.jumping && maxAirJumps >= 1 && Grounded())
        {
            Debug.Log("Primer Salto");
            airJumpCounter = 1;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            pState.jumping = true;
            jumpBufferCounter = 0;

#if !UNITY_STANDALONE && !UNITY_WEBGL
            jumpButtonPressed = false;
#endif
        }
        //Intentar salto en el aire (doble salto)
        else if (!Grounded() && airJumpCounter < maxAirJumps && (Input.GetButtonDown("Jump") || jumpButtonPressed))
        {
            Debug.Log("Segundo Salto");
            airJumpCounter++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            pState.jumping = true;

#if !UNITY_STANDALONE && !UNITY_WEBGL
            jumpButtonPressed = false;
#endif
        }

        // Cortar salto si se suelta el botón mientras asciende (Variar intensidad de salto)
        if ((Input.GetButtonUp("Jump") || jumpButtonReleased) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
#if !UNITY_STANDALONE && !UNITY_WEBGL
            jumpButtonReleased = false;
#endif
        }

        if (Grounded())
        {
            pState.jumping = false;
        }

        anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            //airJumpCounter = 0;
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
            jumpBufferCounter -= Time.deltaTime * 10;
        }
    }

    public void TakeDamage(float _damage)
    {
        if (!pState.dashing && isAlive)
        {
            health -= _damage;            

            anim.SetTrigger("TakeDamage");
            HitStopTime(0, 5, 0.5f);
            StartCoroutine(stopTakeDamage());
        }


        if (health<=0 && isAlive)
        {
            isAlive = false;
            StartCoroutine(GameOver());
        }
    }

    IEnumerator stopTakeDamage()
    {
        pState.invincible = true;
        GameObject _bloodSpurt = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurt, 1.5f);
        

        anim.SetTrigger("TakeDamage");
        ClampHealth();
        yield return new WaitForSeconds(1);
        pState.invincible = false;        
    }

    IEnumerator GameOver()
    {
        anim.SetTrigger("Death");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Attackable"), true);
        
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    void FlashWhileInvicible()
    {
        spritePlayer.color = pState.invincible ? Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * hitFlashSpeed, 0.8f)) : Color.white;
    }

    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.unscaledDeltaTime  * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float _newTimeScale, int _restoreTime, float _delay)
    {
        restoreTimeSpeed = _restoreTime;
        Time.timeScale = _newTimeScale;

        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float _delay)
    {       
        yield return new WaitForSecondsRealtime(_delay);
        restoreTime = true;
    }
    

    //MÉTODOS PARA LOS BOTONES ANDROID
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
