using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    private float nextSpawnTime = 0f;
    private Camera mainCamera;

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
            SpawnWaveInHiddenPoints();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnWaveInHiddenPoints()
    {
        if (enemyPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (IsPointOutsideCameraView(spawnPoint.position))
                {
                    int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                    Debug.Log($"Pojawi� si� przeciwnik: {enemyPrefab.name} w punkcie: {spawnPoint.name}, kt�ry jest poza widokiem kamery.");
                }
                else
                {
                    Debug.Log($"Punkt spawnu {spawnPoint.name} pomini�ty, znajduje si� w widoku kamery.");
                }
            }
        }
    }

    private bool IsPointOutsideCameraView(Vector3 point)
    {
        if (mainCamera == null) return true;
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Bounds bounds = new Bounds(point, Vector3.one * 0.1f);
        return !GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
    }
}
