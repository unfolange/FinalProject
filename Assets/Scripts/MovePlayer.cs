
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D rd2D;
    private float horizontalMove = 0f;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float movementSmoothing = 0.05f;

    private Vector2 currentVelocity = Vector2.zero;
    [SerializeField] private bool seeRight = true;

    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxDimention;
    [SerializeField] private bool isGrounded;
    private bool jump = false;

    private void Start()
    {
        rd2D = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxDimention, 0f, groundLayer);

        Move(horizontalMove + Time.fixedDeltaTime, jump);
        jump = false;

    }

    private void Move(float targetHorizontalSpeed, bool jump)
    {
        if (rd2D == null) return;


        Vector2 targetVelocity = new Vector2(targetHorizontalSpeed, rd2D.linearVelocity.y);


        rd2D.linearVelocity = Vector2.SmoothDamp(rd2D.linearVelocity, targetVelocity, ref currentVelocity, movementSmoothing);


        if (targetHorizontalSpeed > 0 && !seeRight)
        {
            Turn();
        }
        else if (targetHorizontalSpeed < 0 && seeRight)
        {
            Turn();
        }
        if (jump && isGrounded)
        {
            isGrounded = false;
            rd2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Turn()
    {
        seeRight = !seeRight;


        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, boxDimention);
    }
}
