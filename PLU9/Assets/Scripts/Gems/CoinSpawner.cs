using UnityEngine;
using System.Collections.Generic;

// 코인 생성 패턴을 정의합니다.
[System.Serializable]
public class CoinPattern
{
    public string patternName;
    public List<Vector3> coinOffsets;
}

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // 코인 프리팹
    public Transform spawnPoint; // 생성 위치
    public float minSpawnInterval = 3.0f;
    public float maxSpawnInterval = 5.0f;
    public float coinScrollSpeed = 5f; // 생성될 코인의 이동 속도
    public Transform playerTransform;

    public List<CoinPattern> coinPatterns;

    private float timer;
    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnInterval)
        {
            SpawnCoinPattern();
            currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            timer = 0f;
        }
    }

    void SpawnCoinPattern()
    {
        if (coinPrefab == null || spawnPoint == null || coinPatterns.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, coinPatterns.Count);
        CoinPattern selectedPattern = coinPatterns[randomIndex];

        foreach (Vector3 offset in selectedPattern.coinOffsets)
        {
            Vector3 spawnPosition = spawnPoint.position + offset;
            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            Coin coinComponent = newCoin.GetComponent<Coin>();
            if (coinComponent != null)
            {
                coinComponent.scrollSpeed = coinScrollSpeed;
                coinComponent.SetPlayerTransform(playerTransform);
            }
        }
    }
}
