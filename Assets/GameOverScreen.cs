using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameOverScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private void OnEnable()
    {
        // Sprawdzanie, czy Video Player istnieje i uruchamianie filmu
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    private void OnDisable()
    {
        // Zatrzymywanie filmu, gdy Canvas jest wyłączany
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

}
