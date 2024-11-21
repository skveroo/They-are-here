using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float lowHealth = 0f; 
    public float health = 100f;
    public string enemyTag = "Enemy";
    public HealthBar healthBar;


    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
            endConditions.NotifyObjectDestroyed(gameObject);
            Destroy(gameObject);   
    }
}

