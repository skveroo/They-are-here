using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;   // Komponent NavMeshAgent
    private Transform player;     // Transform gracza

    [SerializeField] private float detectionRadius = 50f;  // Zasięg detekcji gracza
    [SerializeField] private float randomPointRange = 3f;  // Zakres losowego punktu wokół gracza
    [SerializeField] private float separationRadius = 2f;  // Promień separacji od innych wrogów
    [SerializeField] private float repathTime = 1f;        // Czas pomiędzy aktualizacjami ścieżki

    private float nextPathUpdate = 0f;
    private bool isCollidingWithPlayer = false; // Flag to track collision status

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Znalezienie gracza po tagu "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Brak obiektu gracza z tagiem 'Player'");
        }
    }

    void Update()
    {
        if (player != null && !isCollidingWithPlayer)  // Only move if not colliding with the player
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Jeśli gracz jest w zasięgu detekcji, AI porusza się w jego kierunku
            if (distanceToPlayer <= detectionRadius && Time.time >= nextPathUpdate)
            {
                Vector3 targetPosition = GetRandomPointAroundPlayer();
                Vector3 separation = GetSeparationVector();

                // Dodanie wektora separacji do celu
                agent.SetDestination(targetPosition + separation);

                nextPathUpdate = Time.time + repathTime;
            }
        }
    }

    /// <summary>
    /// Losuje punkt wokół gracza w określonym zakresie.
    /// </summary>
    Vector3 GetRandomPointAroundPlayer()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomPointRange;
        randomDirection.y = 0;  // Ustawienie na płaszczyźnie
        Vector3 randomPoint = player.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, randomPointRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return player.position; // Wróć do gracza, jeśli nie znaleziono punktu
    }

    /// <summary>
    /// Oblicza wektor separacji, aby uniknąć kolizji z innymi wrogami.
    /// </summary>
    Vector3 GetSeparationVector()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius);
        Vector3 separationVector = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject != this.gameObject && neighbor.CompareTag("Enemy"))
            {
                Vector3 awayFromNeighbor = transform.position - neighbor.transform.position;
                separationVector += awayFromNeighbor.normalized / awayFromNeighbor.magnitude;
            }
        }

        return separationVector.normalized * separationRadius;
    }

    // Detects collision with an object tagged as "Player" and stops movement
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true; // Stop movement on collision with player
            agent.isStopped = true;
        }
    }

    // Detects when the collision ends (player is no longer colliding)
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false; // Resume movement when player moves away
            agent.isStopped = false;
        }
    }
}
