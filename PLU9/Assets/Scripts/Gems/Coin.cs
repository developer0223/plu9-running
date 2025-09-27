using UnityEngine;

public class Coin : MonoBehaviour
{
    public float scrollSpeed = 5f; // 이동속도
    public float destroyXPosition = -10f; // 파괴 X 좌표

    [Header("Magnet Effect")]
    public float magnetDistance = 3f;
    public float magnetSpeed = 10f;

    private Transform playerTransform;
    private bool isMagnetized = false;

    // Spawner가 플레이어 Transform을 설정할 수 있도록 하는 메서드
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
            }
        }
        else
        {
            if (playerTransform != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
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
            CollectCoin();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddCoins(1); // 코인 카운터에 1 추가
        }

        Destroy(gameObject);
    }
}
