using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Dodajemy, by korzystać z TextMeshPro

public class PlayerExperience : MonoBehaviour
{
    public Slider experienceSlider;  // Referencja do paska doświadczenia
    public TextMeshProUGUI levelText;  // Referencja do tekstu poziomu
    public float maxExperience = 100;  // Maksymalna wartość doświadczenia
    private int currentExperience = 0;  // Aktualne doświadczenie
    private int currentLevel = 1;  // Aktualny poziom gracza

    void Start()
    {
        // Ustawienie początkowej wartości paska
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

        // Zwiększanie wymagań na kolejny poziom o 40% w stosunku do poprzedniego
        maxExperience = maxExperience * 1.4f;

        // Aktualizacja paska doświadczenia, aby nowy poziom był wymagający
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
