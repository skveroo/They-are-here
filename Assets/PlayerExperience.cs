using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    public Slider experienceSlider;       // Referencja do paska doświadczenia

    public TextMeshProUGUI levelText;     // Referencja do tekstu poziomu
    public float maxExperience = 100;     // Maksymalna wartość doświadczenia
    private int currentExperience = 0;    // Aktualne doświadczenie
    private int currentLevel = 1;         // Aktualny poziom gracza

    public HealthBar health;      // referencja do komponentu HealthBar
    public float healthIncreaseAmount = 1.1f; // Ilość zwiększanego zdrowia przy awansie

    private Health playerHealth;          // Referencja do komponentu Health

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

        // Ograniczenie doświadczenia do maksymalnej wartości
        if (currentExperience >= maxExperience)
        {
            currentExperience = 0;
            LevelUp();
        }

        // Aktualizacja paska doświadczenia
        UpdateExperienceBar();
    }

    private void LevelUp()
    {
        currentLevel++;  // Zwiększenie poziomu
        Debug.Log("Level Up! Nowy poziom: " + currentLevel);

        // Zwiększenie maksymalnego zdrowia gracza oraz przywrócenie do pełni zdrowia
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealthAndUpdateHealth(healthIncreaseAmount);
            health.SetHealth(playerHealth.health);
            
        }

        // Zwiększenie wymagań na kolejny poziom o 40%
        maxExperience = maxExperience * 1.4f;
        // Aktualizacja paska doświadczenia do nowego poziomu
        experienceSlider.maxValue = maxExperience;
        // Aktualizacja tekstu poziomu
        UpdateLevelText();

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
