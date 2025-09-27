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
    private bool isDead = false; // 플레이어 사망 여부

    [SerializeField] private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = 0;
        InitButton();
        isDead = false;
    }

    void Update()
    {
        if (isDead) return; // 사망 시 업데이트 중지

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
        if (isDead) return; // 사망 시 점프 불가

        if (jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 디버깅을 위해 충돌한 오브젝트의 이름과 태그를 콘솔에 출력합니다.
        Debug.Log("Triggered by: " + other.gameObject.name + ", Tag: " + other.gameObject.tag);

        if (isDead) return;

        // "Obstacle" 태그를 가진 오브젝트와 충돌했는지 확인
        if (other.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            playerAnimator.SetTrigger("isDead"); // "isDead" 트리거로 사망 애니메이션 실행

            // 플레이어 물리적 움직임 정지
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // 다른 물리적 상호작용 방지

            // GameManager에 게임 오버 알림
            GameManager.Instance.GameOver();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
