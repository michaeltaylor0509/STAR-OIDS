using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour {
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle hudToggle;
    [SerializeField] private GameObject hudCanvas;

    void OnEnable() {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        volumeSlider.SetValueWithoutNotify(savedVolume);
        fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen);

        if (hudToggle != null) {
            bool savedShowHUD = PlayerPrefs.GetInt("ShowHUD", 1) == 1;
            hudToggle.SetIsOnWithoutNotify(savedShowHUD);
        }
    }

    public void OnVolumeChanged(float value) {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void OnFullscreenChanged(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnShowHUDChanged(bool show) {
        if (hudCanvas != null) {
            hudCanvas.SetActive(show);
        }
        PlayerPrefs.SetInt("ShowHUD", show ? 1 : 0);
        PlayerPrefs.Save();
    }
}