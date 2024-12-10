using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float detectionRadius = 50f;

    private float nextSpawnTime = 0f;
    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Brak obiektu gracza z tagiem 'Player'");
        }

        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("Brak przypisanych prefabów przeciwników w spawnerze!");
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Brak ustawionych punktów spawnu!");
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWaveIfPlayerInRange();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnWaveIfPlayerInRange()
    {
        if (enemyPrefabs.Length > 0 && spawnPoints.Length > 0 && player != null)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);
                if (distanceToPlayer <= detectionRadius)
                {
                    int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                    Debug.Log($"Pojawi³ siê przeciwnik: {enemyPrefab.name} w punkcie: {spawnPoint.name}, gracz w zasiêgu ({distanceToPlayer:F2}m)");
                }
            }
        }
    }
}
