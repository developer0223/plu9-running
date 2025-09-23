using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; 
    public Transform spawnPoint; // 장애물이 생성 위치
    public float minSpawnInterval = 1.5f;
    public float maxSpawnInterval = 3.0f;
    public float obstacleScrollSpeed = 5f; // 생성될 장애물의 이동 속도 

    private float timer;
    private float currentSpawnInterval; 

    void Start()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval); // 첫 생성 주기 설정
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnObstacle();
            currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval); 
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomIndex];

        GameObject newObstacle = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        Obstacle obstacleComponent = newObstacle.GetComponent<Obstacle>();
        if (obstacleComponent != null)
        {
            obstacleComponent.scrollSpeed = obstacleScrollSpeed;
        }
       
    }
}