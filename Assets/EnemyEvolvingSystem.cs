using System.Collections.Generic;
using UnityEngine;

public class EnemyEvolvingSystem : MonoBehaviour
{
    public float healthIncreaseAmount = 1.4f; // Współczynnik wzrostu zdrowia
    public float damageIncreaseAmount = 1.7f; // Współczynnik wzrostu obrażeń

    private List<Health> enemiesHealth = new List<Health>(); // Lista zdrowia przeciwników
    private List<DamageDealer> enemiesDamage = new List<DamageDealer>(); // Lista obrażeń przeciwników

    private void Start()
    {
        // Opcjonalnie: znajdź wszystkich przeciwników na początku
        RegisterAllEnemies();
    }

    public void RegisterEnemy(Health enemyHealth, DamageDealer enemyDamage)
    {
        if (!enemiesHealth.Contains(enemyHealth))
        {
            enemiesHealth.Add(enemyHealth);
        }
        if (!enemiesDamage.Contains(enemyDamage))
        {
            enemiesDamage.Add(enemyDamage);
        }
    }

    public void LevelUpEnemies()
    {
        foreach (var enemyHealth in enemiesHealth)
        {
            enemyHealth.maxHealth *= healthIncreaseAmount; // Aktualizacja obecnego zdrowia (jeśli metoda istnieje)
        }

        foreach (var enemyDamage in enemiesDamage)
        {
            enemyDamage.damageAmount *= damageIncreaseAmount;
        }
    }

    private void RegisterAllEnemies()
    {
        var enemyHealthComponents = FindObjectsOfType<Health>();
        foreach (var enemyHealth in enemyHealthComponents)
        {
            var enemyDamage = enemyHealth.GetComponent<DamageDealer>();
            if (enemyDamage != null)
            {
                RegisterEnemy(enemyHealth, enemyDamage);
            }
        }
    }
}
