using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float health = 100f;
    public string enemyTag = "Enemy";

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has died, calling GameOver.");
            Destroy(gameObject);

        }
        else if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy has been destroyed, calling GameWin.");
            Destroy(gameObject);

        }
        else
        {
            Destroy(gameObject);

        }
    }
}

