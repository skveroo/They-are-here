using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool isDefeated = false;
    public GameObject mainObjective; // Możesz przypisać go w inspektorze lub stworzyć dynamicznie.

    void Awake()
    {
        // Tworzymy obiekt dynamicznie, jeśli nie został przypisany w inspektorze
        if (mainObjective == null)
        {
            mainObjective = new GameObject("MainObjective");
            mainObjective.tag = "mainObjective"; // Ustawiamy odpowiedni tag
        }
    }

    void OnDestroy()
    {
        if (!isDefeated)
        {
            OnBossDefeated();
        }
    }

    public void OnBossDefeated()
    {
        Debug.Log("Finalny boss został pokonany! Gratulacje! Koniec gry.");
        isDefeated = true;

        // Powiadom system zakończenia gry
        endConditions.NotifyObjectDestroyed(mainObjective);

        // Zatrzymaj grę
        Time.timeScale = 0f;
    }
}
