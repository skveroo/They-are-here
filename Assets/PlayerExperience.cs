using UnityEngine;
using TMPro;  // Upewnij się, że masz tę przestrzeń nazw

public class PlayerExperience : MonoBehaviour
{
    public int experience = 0;  // Aktualne doświadczenie
    public TextMeshProUGUI experienceText;  // Referencja do komponentu TextMeshProUGUI

    // Funkcja do dodawania doświadczenia
    public void AddExperience(int amount)
    {
        experience += amount;
        UpdateExperienceUI();
    }

    // Funkcja do aktualizacji tekstu w UI
    private void UpdateExperienceUI()
    {
        if (experienceText != null)
        {
            experienceText.text = "Experience: " + experience.ToString();
        }
    }
}
