using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private Health healthComponent;
    public Transform transformObject;

    private void Start()
    {
        // Jesli HealthBar jest przypisany do gracza, znajdz jego komponent zdrowia
        if (gameObject.CompareTag("Player"))
        {
            healthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        else
        {
            // Przypisz komponent Health do obiektu, do ktorego jest przypiety pasek zdrowia (np. Enemy)
            healthComponent = GetComponentInParent<Health>();
        }

        if (healthComponent != null && healthBar != null)
        {
            healthBar.maxValue = healthComponent.health;
            healthBar.value = healthComponent.health;
        }
        else
        {
            Debug.LogError("Health component or HealthBar Slider not found.");
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.identity; // Brak rotacji

    }

    public void SetHealth(float hp)
    {
        if (healthBar != null)
        {
            healthBar.value = hp;
        }
    }
}
