using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle; // Referencja do Toggle

public TMPro.TMP_Dropdown qualityDropdown; // Dodaj Dropdown do ustawień jakości
    private Resolution[] resolutions;

    void Start()
    {

        resolutions = Screen.resolutions;
        // Wczytywanie zapisanej rozdzielczości i ustawienia
        int savedResolutionWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
        int savedResolutionHeight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        int savedQuality = PlayerPrefs.GetInt("Quality", 2);

    Debug.Log($"Saved Resolution: {savedResolutionWidth}x{savedResolutionHeight}");
    Debug.Log($"Saved Fullscreen: {savedFullscreen}");
    Debug.Log($"Saved Quality: {savedQuality}");
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // Znajdowanie indeksu zapisanej rozdzielczości
            if (resolutions[i].width == savedResolutionWidth && resolutions[i].height == savedResolutionHeight)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        // Ustawienie toggle dla fullscreen
        fullscreenToggle.isOn = savedFullscreen;

        // Podpięcie funkcji zmieniającej fullscreen do toggle
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Ustawienie rozdzielczości i trybu pełnoekranowego na starcie
        Screen.SetResolution(savedResolutionWidth, savedResolutionHeight, savedFullscreen);

        qualityDropdown.value = savedQuality;
        SetQuality(savedQuality); // Zastosowanie ustawionej jakości
        qualityDropdown.onValueChanged.AddListener(SetQuality); // Podpięcie zmiany jakości
    }
    

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex); // Zapisz poziom jakości
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0); // Zapisz ustawienie fullscreen
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Zapisz ustawienie rozdzielczości
        PlayerPrefs.SetInt("ResolutionWidth", resolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", resolution.height);
    }
}
