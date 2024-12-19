using UnityEngine;
using TMPro;

public class GameStatsDisplay : MonoBehaviour
{
    public TMP_Text statsText; // Przypisz Text z Canvasa w Inspectorze
    public PlayerExperience expTaken;
    public Weapon dmgDealt;
    public Health totalTimePlayed;

    private bool statsDisplayed = false; // Flaga zapobiegająca wielokrotnemu generowaniu statystyk

    public void ShowStats()
    {
        if (!statsDisplayed)
        {
            if (statsText != null)
            {
                statsText.text = GenerateStatsText();
                statsDisplayed = true;
            }
            else
            {
                Debug.LogError("StatsText is not assigned in the Inspector.");
            }
        }
    }

    // Funkcja do generowania tekstu statystyk
    private string GenerateStatsText()
    {
        if (totalTimePlayed == null || dmgDealt == null || expTaken == null)
        {
            Debug.LogError("One or more required references are not assigned.");
            return "Error: Missing required data.";
        }

        // Formatowanie czasu w HH:MM:SS
        string formattedTime = FormatTime(totalTimePlayed.totalTimePlayed);

        // Tworzenie treści statystyk
        return $"Total Time Played: {formattedTime}\n" +
               $"Total Damage Dealt: {dmgDealt.damageDealt}\n" +
               $"Experience Gained: {expTaken.expTaken}\n" +
               $"Total Damage Taken: {StatsManager.Instance.totalDamageTaken}\n";
    }

    // Funkcja pomocnicza do formatowania czasu
    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
