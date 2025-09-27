using UnityEngine;

public class Gem : MonoBehaviour
{
    public float scrollSpeed = 5f; // 이동속도
    public float destroyXPosition = -10f; // Gem 파괴 X 좌표 
    public int scoreValue = 100; 

    [Header("Magnet Effect") ]
    public float magnetDistance = 3f; 
    public float magnetSpeed = 10f; 
    public float magnetCurveHeight = 1f; // 자석 효과 시 곡선 경로의 최대 높이 (곡선의 정점)
    public float magnetCurveDuration = 0.5f; // 자석 효과 시작부터 최고점에 도달하는 시간 (대략적인 곡선 지속 시간)

    private Transform playerTransform;
    private bool isMagnetized = false;
    private Vector3 magnetStartPos; // 자석 효과 시작 시 Gem의 위치
    private float magnetTimer = 0f; // 자석 효과 진행 시간

    // GemSpawner가 플레이어 Transform을 설정할 수 있도록 하는 메서드
    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    void Update()
    {
        if (!isMagnetized)
        {
           
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < magnetDistance)
            {
                isMagnetized = true;
                magnetStartPos = transform.position; // 자석 효과 시작 위치 저장
                magnetTimer = 0f; 
            }
        }
        else
        {
            if (playerTransform != null)
            {
                magnetTimer += Time.deltaTime;
                float t = magnetTimer / magnetCurveDuration;
                t = Mathf.Clamp01(t); // 

                float currentYOffset = Mathf.Sin(t * Mathf.PI) * magnetCurveHeight;

                Vector3 targetPositionThisFrame = playerTransform.position;
                targetPositionThisFrame.y += currentYOffset;

                transform.position = Vector3.MoveTowards(transform.position, targetPositionThisFrame, magnetSpeed * Time.deltaTime);
            }
        }

        if (transform.position.x < destroyXPosition)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectGem();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectGem();
        }
    }

    void CollectGem()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            ScoreManager.Instance.AddGems(1); // 보석 카운터에 1 추가
        }
        
        Destroy(gameObject);
    }
}