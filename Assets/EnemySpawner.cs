using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject finalBossPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    private Camera mainCamera;
    private float nextSpawnTime = 0f;
    private int waveCount = 0;
    private bool bossSpawned = false;

    void Start()
    {
        mainCamera = Camera.main;

        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("Brak przypisanych prefab�w przeciwnik�w w spawnerze!");
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Brak ustawionych punkt�w spawnu!");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Nie znaleziono g��wnej kamery w scenie!");
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            waveCount++;
            Debug.Log($"Fala {waveCount} rozpocz�ta!");

            SpawnWaveInHiddenPoints();

            if (waveCount >= 5 && !bossSpawned)
            {
                SpawnFinalBoss();
                bossSpawned = true;
            }

            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnWaveInHiddenPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (IsPointOutsideCameraView(spawnPoint.position))
            {
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                Debug.Log($"Pojawi� si� przeciwnik: {enemyPrefab.name} w punkcie: {spawnPoint.name}.");
            }
            else
            {
                Debug.Log($"Punkt spawnu {spawnPoint.name} pomini�ty (w widoku kamery).");
            }
        }
    }

    private void SpawnFinalBoss()
    {
        Debug.Log("Finalny boss pojawi� si�!");

        Transform spawnPoint = GetRandomHiddenSpawnPoint();
        if (spawnPoint != null)
        {
            GameObject boss = Instantiate(finalBossPrefab, spawnPoint.position, Quaternion.identity);

            if (boss.GetComponent<BossController>() == null)
            {
                boss.AddComponent<BossController>();
            }
        }
    }

    private Transform GetRandomHiddenSpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (IsPointOutsideCameraView(spawnPoint.position))
            {
                return spawnPoint;
            }
        }

        return null;
    }

    private bool IsPointOutsideCameraView(Vector3 point)
    {
        if (mainCamera == null) return true;

        // Pobierz obszar widzenia kamery (frustum)
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Stw�rz niewielki AABB (o� spawnu jako punkt)
        Bounds bounds = new Bounds(point, Vector3.one * 0.1f);

        // Sprawd�, czy AABB jest poza obszarem widzenia kamery
        return !GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
    }
}