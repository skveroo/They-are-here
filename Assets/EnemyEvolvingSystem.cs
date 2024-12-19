using System.Collections.Generic;
using UnityEngine;

public class EnemyEvolvingSystem : MonoBehaviour
{
   public float healthIncreaseAmount = 1.5f; // Współczynnik skalowania zdrowia
    public float damageIncreaseAmount = 1.25f; // Współczynnik skalowania obrażeń

    private Health enemyHealth;
    private DamageDealer enemyDamage;
    private PlayerExperience playerExperience;

    private void Start()
    {
        // Znalezienie komponentów Health i DamageDealer
        enemyHealth = GetComponent<Health>();
        enemyDamage = GetComponent<DamageDealer>();

        // Znalezienie gracza i jego poziomu doświadczenia
        playerExperience = FindObjectOfType<PlayerExperience>();

        if (playerExperience != null && enemyHealth != null && enemyDamage != null)
        {
            ScaleStats(playerExperience.currentLevel);
        }
    }

    private void ScaleStats(int playerLevel)
    {
        // Skalowanie zdrowia
        //float healthMultiplier = Mathf.Pow(healthIncreaseAmount, playerLevel - 1);
        enemyHealth.maxHealth = enemyHealth.maxHealth * healthIncreaseAmount; 
        enemyHealth.health = enemyHealth.maxHealth; // Aktualizacja obecnego zdrowia do maksymalnego

        // Skalowanie obrażeń
        //float damageMultiplier = Mathf.Pow(damageIncreaseAmount, playerLevel - 1);
        enemyDamage.damageAmount = enemyDamage.damageAmount * damageIncreaseAmount;
    }
}
