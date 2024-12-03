using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float lowHealth = 0f; // Minimalne zdrowie
    public float health = 100f;  // Aktualne zdrowie
    public string enemyTag = "Enemy"; // Tag przeciwnika
    public HealthBar healthBar; // Referencja do paska zdrowia
    public GameObject experienceOrbPrefab; // Prefab kulki doświadczenia
    public int numberOfOrbs = 3; // Liczba kulek doświadczenia do wygenerowania
    public int experiencePerOrb = 5;  // Ilość doświadczenia za jedną kulkę

    // Funkcja przyjmowania obrażeń
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);  // Ustawienie paska zdrowia

        if (health <= 0)
        {
            Die();  // Wywołanie śmierci, jeśli zdrowie wynosi 0
        }
    }

    // Funkcja śmierci
    private void Die()
    {
        // Jeśli to przeciwnik, generujemy kulki doświadczenia
        if (tag == enemyTag)
        {
            GenerateExperienceOrbs();
        }

        // Zniszczenie obiektu (przeciwnika)
        Destroy(gameObject);
    }

    // Generowanie kulek doświadczenia po śmierci
    private void GenerateExperienceOrbs()
    {
        if (experienceOrbPrefab == null)
        {
            Debug.LogWarning("Brak prefabu kulki doświadczenia!");
            return;
        }

        for (int i = 0; i < numberOfOrbs; i++)
        {
            // Losowanie pozycji w pobliżu przeciwnika
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;
            spawnPosition.y = transform.position.y;  // Ustawienie wysokości na poziomie przeciwnika

            // Tworzenie kulki doświadczenia
            GameObject orb = Instantiate(experienceOrbPrefab, spawnPosition, Quaternion.identity);

            // Przekazanie wartości doświadczenia kulce
            ExperienceOrb experienceOrbScript = orb.GetComponent<ExperienceOrb>();
            if (experienceOrbScript != null)
            {
                experienceOrbScript.experienceAmount = experiencePerOrb;
            }
        }
    }
}
