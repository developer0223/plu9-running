using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    private int currentScore = 0;
    private int gemsCollected = 0;  // 획득한 보석 개수
    private int coinsCollected = 0; // 획득한 코인 개수

    // 다른 스크립트에서 값을 읽기 위한 프로퍼티
    public int CurrentScore => currentScore;
    public int GemsCollected => gemsCollected;
    public int CoinsCollected => coinsCollected;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gemsCollected = 0;
        coinsCollected = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    public void AddGems(int amount)
    {
        gemsCollected += amount;
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
