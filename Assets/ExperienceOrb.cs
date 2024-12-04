using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public int experienceAmount = 10;  // Ilość doświadczenia, które daje kulka
    public float moveSpeed = 5f;  // Prędkość przyciągania kulki do gracza
    public float attractionRadius = 3f;  // Promień, w którym kulka zaczyna się przyciągać

    private Transform player;  // Transform gracza
    private bool isCollected = false;  // Flaga, czy kulka została zebrana

    void Start()
    {
        // Znalezienie gracza po tagu "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && !isCollected)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Jeśli gracz jest w zasięgu, kulka zaczyna się przyciągać
            if (distanceToPlayer < attractionRadius)
            {
                MoveTowardsPlayer();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Płynne przesuwanie kulki w stronę gracza
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzenie, czy kolizja jest z graczem i czy kulka nie została już zebrana
        if (other.CompareTag("Player") && !isCollected)
        {
            // Dodanie doświadczenia graczowi
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.AddExperience(experienceAmount);
            }

            // Oznaczenie kulki jako zebranej i zniszczenie jej
            isCollected = true;
            Destroy(gameObject);
        }
    }
}
