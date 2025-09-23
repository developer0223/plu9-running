using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GemPattern
{
    public string patternName; 
    public List<Vector3> gemOffsets; 
}

public class GemSpawner : MonoBehaviour
{
    public GameObject gemPrefab; 
    public Transform spawnPoint; 
    public float minSpawnInterval = 2.5f; 
    public float maxSpawnInterval = 4.0f; 
    public float gemScrollSpeed = 5f; // 생성될 Gem의 이동 속도
    public Transform playerTransform; 

    public List<GemPattern> gemPatterns;

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
            SpawnGemPattern();
            currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            timer = 0f;
        }
    }

    void SpawnGemPattern()
    {
        if (gemPrefab == null)
        {
            return;
        }
        if (spawnPoint == null)
        {
            return;
        }
        if (gemPatterns.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, gemPatterns.Count);
        GemPattern selectedPattern = gemPatterns[randomIndex];

        foreach (Vector3 offset in selectedPattern.gemOffsets)
        {            
            Vector3 spawnPosition = spawnPoint.position + offset;
            GameObject newGem = Instantiate(gemPrefab, spawnPosition, Quaternion.identity);

            Gem gemComponent = newGem.GetComponent<Gem>();
            if (gemComponent != null)
            {
                gemComponent.scrollSpeed = gemScrollSpeed;
                gemComponent.SetPlayerTransform(playerTransform);
            }
        }
    }
}