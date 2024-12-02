using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;  // Komponent NavMeshAgent
    private Transform player;   // Transform gracza

    void Start()
    {
        // Pobranie komponentu NavMeshAgent
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
        // Jeœli gracz istnieje, ustaw jego pozycjê jako cel
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}