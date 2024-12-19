using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    public float totalDamageTaken = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Unikaj wielu instancji StatsManager
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Utrzymanie StatsManager między scenami
    }

    public void AddDamageTaken(float damage)
    {
        totalDamageTaken += damage;
        Debug.Log($"Total Damage Taken: {totalDamageTaken}");
    }

    public float GetTotalDamageTaken()
    {
        return totalDamageTaken;
    }
}
