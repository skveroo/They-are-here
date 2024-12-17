using UnityEngine;

public class BossController : MonoBehaviour
{
    private bool isDefeated = false;

    void OnDestroy()
    {
        if (!isDefeated)
        {
            OnBossDefeated();
            isDefeated = true;
        }
    }

    private void OnBossDefeated()
    {
        Debug.Log("Finalny boss zosta³ pokonany! Gratulacje! Koniec gry.");
        Time.timeScale = 0f;
    }
}

