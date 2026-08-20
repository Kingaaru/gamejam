using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelGenerator : MonoBehaviour
{
    [Header("Core References")]
    public Transform player;
    public GameObject groundPrefab;
    
    [Header("Obstacle Prefabs")]
    public GameObject jumpObstaclePrefab; 
    public GameObject duckObstaclePrefab; 
    public GameObject qteHandPrefab;      

    [Header("Manual Spawn Offsets (Fine-tune position here!)")]
    [Tooltip("Tweak these values in the Inspector to fix floating/sinking items")]
    public Vector3 jumpOffset = new Vector3(0, 1f, 0); 
    public Vector3 duckOffset = new Vector3(0, 3f, 0); 
    public Vector3 qteHandOffset = new Vector3(0, 1.5f, 0); 

    [Header("Generation Settings")]
    public float platformLength = 50f; 
    public int platformsOnScreen = 5;  
    public float spawnChance = 0.75f; 
    
    private float spawnX = 0f;
    private List<GameObject> activePlatforms = new List<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        spawnX = player.position.x + (platformLength / 2f); 

        for (int i = 0; i < platformsOnScreen; i++)
        {
            SpawnPlatform(i == 0); 
        }
    }

    void Update()
    {
        // 1. SPAWN AHEAD
        while (spawnX < player.position.x + (platformLength * platformsOnScreen))
        {
            SpawnPlatform(false);
        }

        // 2. CLEANUP PLATFORMS BEHIND
        if (activePlatforms.Count > 0)
        {
            if (activePlatforms[0].transform.position.x < player.position.x - (platformLength * 1.5f))
            {
                Destroy(activePlatforms[0]);
                activePlatforms.RemoveAt(0);
            }
        }

        // 3. CLEANUP OBSTACLES BEHIND
        if (activeObstacles.Count > 0)
        {
            if (activeObstacles[0] == null) 
            {
                activeObstacles.RemoveAt(0);
            }
            else if (activeObstacles[0].transform.position.x < player.position.x - (platformLength * 1.5f))
            {
                Destroy(activeObstacles[0]);
                activeObstacles.RemoveAt(0);
            }
        }
    }

    private void SpawnPlatform(bool isEmpty)
    {
        // Ground spawns exactly at Y = -1
        GameObject newPlatform = Instantiate(groundPrefab, new Vector3(spawnX, -1f, 0), Quaternion.identity);
        activePlatforms.Add(newPlatform);

        if (!isEmpty && Random.value <= spawnChance)
        {
            // Safely check which obstacles exist
            List<int> availableIndices = new List<int>();
            if (jumpObstaclePrefab != null) availableIndices.Add(0);
            if (duckObstaclePrefab != null) availableIndices.Add(1);
            if (qteHandPrefab != null) availableIndices.Add(2);

            if (availableIndices.Count > 0)
            {
                int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
                
                GameObject prefabToSpawn = null;
                Vector3 selectedOffset = Vector3.zero;

                // Match the chosen obstacle with your custom Inspector offsets
                if (randomIndex == 0) { prefabToSpawn = jumpObstaclePrefab; selectedOffset = jumpOffset; }
                else if (randomIndex == 1) { prefabToSpawn = duckObstaclePrefab; selectedOffset = duckOffset; }
                else if (randomIndex == 2) { prefabToSpawn = qteHandPrefab; selectedOffset = qteHandOffset; }

                float randomXPosition = spawnX + Random.Range(-platformLength / 3f, platformLength / 3f);
                
                // Spawn position = The floor's exact position + Your custom offset
                Vector3 spawnPos = new Vector3(randomXPosition, -1f, 0) + selectedOffset;
                
                GameObject newObstacle = Instantiate(prefabToSpawn, spawnPos, prefabToSpawn.transform.rotation);
                activeObstacles.Add(newObstacle);
            }
        }

        spawnX += platformLength;
    }
}