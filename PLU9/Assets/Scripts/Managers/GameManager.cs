using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Objects")]
    public GameObject playerObject;      // 인게임 플레이어
    public BgScroller bgScroller;        // 배경 스크롤러
    public GemSpawner gemSpawner;        // 보석 생성기
    public CoinSpawner coinSpawner;      // 코인 생성기
    public ObstacleSpawner obstacleSpawner; // 장애물 생성기

    [Header("UI Elements")]
    public GameObject jumpButtonObject;    // 점프 버튼 오브젝트
    public GameObject healthUIObject;      // 체력 UI 오브젝트
    public GameObject resultPanel;       // 결과창 UI
    public GameObject resultPlayerCharacter; // 결과창 캐릭터
    public Image fadeImage;              // 화면 페이드 효과에 사용할 이미지
    public Text finalScoreText;          // 최종 점수 텍스트
    public Text finalCoinsText;          // 최종 코인 개수 텍스트
    public Button retryButton;           // 다시하기 버튼
    public Button quitButton;            // 게임 종료 버튼

    [Header("Game Settings")]
    public float fadeDuration = 1f;      // 페이드 효과 지속 시간
    public float deathAnimationDelay = 1.5f; // 죽는 애니메이션 대기 시간

    private void Awake()
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
        // 게임 시작 시 초기화
        resultPanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);
        if (resultPlayerCharacter != null) resultPlayerCharacter.SetActive(false);

        // 버튼 리스너 할당
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryGame);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void GameOver()
    {
        // 배경 스크롤 중지
        if (bgScroller != null) bgScroller.enabled = false;

        // 모든 Spawner 중지
        if (gemSpawner != null) gemSpawner.enabled = false;
        if (coinSpawner != null) coinSpawner.enabled = false;
        if (obstacleSpawner != null) obstacleSpawner.enabled = false;

        // 화면에 있는 모든 Gem, Coin, Obstacle의 움직임 중지
        foreach (var gem in FindObjectsOfType<Gem>()) { gem.enabled = false; }
        foreach (var coin in FindObjectsOfType<Coin>()) { coin.enabled = false; }
        foreach (var obstacle in FindObjectsOfType<Obstacle>()) { obstacle.enabled = false; }

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 1. 플레이어 죽는 애니메이션 시간만큼 대기
        yield return new WaitForSeconds(deathAnimationDelay);

        // 2. 화면을 어둡게 (Fade Out)
        yield return StartCoroutine(FadeOut());

        // 3. 인게임 UI 및 플레이어 비활성화 (화면이 까만 상태)
        if (jumpButtonObject != null) jumpButtonObject.SetActive(false);
        if (healthUIObject != null) healthUIObject.SetActive(false);
        if (playerObject != null) playerObject.SetActive(false);

        // 4. 결과창 UI 및 캐릭터 활성화
        if (finalScoreText != null) finalScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
        if (finalCoinsText != null) finalCoinsText.text = ScoreManager.Instance.CoinsCollected.ToString();
        resultPanel.SetActive(true);
        if (resultPlayerCharacter != null) resultPlayerCharacter.SetActive(true);

        // 5. 화면을 다시 밝게 (Fade In)
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        fadeImage.color = endColor;
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color startColor = new Color(0, 0, 0, 1);
        Color endColor = new Color(0, 0, 0, 0);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        fadeImage.color = endColor;
        fadeImage.gameObject.SetActive(false);
    }

    // --- 버튼 메서드 ---

    public void RetryGame()
    {
        // 현재 씬을 다시 로드합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // 에디터에서 실행 중일 경우, 종료되지 않으므로 로그를 출력합니다.
#if UNITY_EDITOR
        Debug.Log("Game Quit! (In Editor)");
#endif
        // 빌드된 게임을 종료합니다.
        Application.Quit();
    }
}
