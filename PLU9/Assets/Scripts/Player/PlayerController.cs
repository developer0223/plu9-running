using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public Button btn_jump;

    private Rigidbody2D rb;
    private int jumpCount;
    private bool isGrounded;

    [SerializeField] private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
        InitButton();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        if(isGrounded)
        {
            playerAnimator.SetBool("isJumping", false);
        }
        else
        {
            playerAnimator.SetBool("isJumping", true);
        }


        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            jumpCount = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnClickJumpButton();
        }
    }

    private void InitButton()
    {
        btn_jump.onClick?.AddListener(OnClickJumpButton);
    }

    public void OnClickJumpButton()
    {
        if (jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
