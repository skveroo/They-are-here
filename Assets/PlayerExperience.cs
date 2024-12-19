using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class PlayerExperience : MonoBehaviour
{
    public Slider experienceSlider;       // Referencja do paska doświadczenia
    public EnemyEvolvingSystem enemyEvolvingSystem;
    public TextMeshProUGUI levelText;     // Referencja do tekstu poziomu
    public float maxExperience = 100;     // Maksymalna wartość doświadczenia
    private int currentExperience = 0;    // Aktualne doświadczenie
    public int currentLevel = 1;         // Aktualny poziom gracza
    public GameObject LevelUpgradeUI;
    public float healthIncreaseAmount = 1.5f; // Ilość zwiększanego zdrowia przy awansie
    public float expTaken = 0f;
    public List<Weapon> weapons;    // Referencja do broni

    public float damageIncreaseAmount = 1.2f;

    private Health playerHealth;          // Referencja do komponentu Health Gracza
    

    void Start()
    {
        // Znalezienie komponentu Health
        playerHealth = GetComponent<Health>();

        // Ustawienie początkowej wartości paska doświadczenia
        if (experienceSlider != null)
        {
            experienceSlider.maxValue = maxExperience;
            experienceSlider.value = currentExperience;
        }

        // Ustawienie początkowego poziomu w UI
        UpdateLevelText();
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        expTaken += amount;
        // Ograniczenie doświadczenia do maksymalnej wartości
        if (currentExperience >= maxExperience)
        {
            currentExperience = 0;
            LevelUp();
        }

        // Aktualizacja paska doświadczenia
        UpdateExperienceBar();
    }

    public void LevelUp()
    {
        currentLevel++;  // Zwiększenie poziomu
        Debug.Log("Level Up! Nowy poziom: " + currentLevel);

        // Zwiększenie maksymalnego zdrowia gracza oraz przywrócenie do pełni zdrowia
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealthAndUpdateHealth(healthIncreaseAmount);
            // Zwiększenie obrażeń każdej broni
            foreach (Weapon weapon in weapons)
                {
                     // Skalowanie obrażeń w oparciu o poziom gracza
                    switch (weapon.name)
                    {
                        case "AutomaticRifle":
                            weapon.damageAmount = damageIncreaseAmount * weapon.damageAmount;
                            break;

                        case "Pistol":
                            weapon.damageAmount = damageIncreaseAmount * weapon.damageAmount;
                            break;

                        default:
                            weapon.damageAmount = 0f;
                            break;
                    }
                }   
        }
    
        // Zwiększenie wymagań na kolejny poziom o 40%
        maxExperience = maxExperience * 1.4f;
        // Aktualizacja paska doświadczenia do nowego poziomu
        experienceSlider.maxValue = maxExperience;
        // Aktualizacja tekstu poziomu
        UpdateLevelText();
        if (currentLevel%5 == 0)
        {
            LevelUpgradeUI.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    private void UpdateExperienceBar()
    {
        if (experienceSlider != null)
        {
            experienceSlider.value = currentExperience;
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
    }
    
}
