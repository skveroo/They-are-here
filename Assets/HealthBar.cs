using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    private Health healthComponent;

    private void Start()
    {
        // Jeśli HealthBar jest przypisany do gracza, znajdź jego komponent zdrowia
        if (gameObject.CompareTag("Player"))
        {
            healthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        else
        {
            // Przypisz komponent Health do obiektu, do którego jest przypięty pasek zdrowia (np. Enemy)
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
        if (hp < 0) hp = 0;  // Upewnij się, że zdrowie nie jest mniejsze od 0
        if (hp > healthBar.maxValue) hp = healthBar.maxValue;  // Zapobiega przekroczeniu maxValue

        healthBar.value = hp;
    }
}

}
