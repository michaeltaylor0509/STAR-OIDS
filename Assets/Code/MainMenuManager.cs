using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject shipsPanel;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private string gameSceneName = "SampleScene";

    // nuevo: static para que otras escenas (como Pausa) puedan "avisarnos"
    // ANTES de que carguemos, de que queremos abrir Config directamente
    public static bool openConfigOnLoad = false;

    void Start() {
        // si venimos de Pausa pidiendo Config directamente, lo abrimos
        // y "apagamos" el aviso para que la proxima vez cargue normal
        if (openConfigOnLoad) {
            openConfigOnLoad = false;
            mainMenuPanel.SetActive(false);
            configPanel.SetActive(true);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenShips() {
        mainMenuPanel.SetActive(false);
        shipsPanel.SetActive(true);
    }

    public void OpenConfig() {
        mainMenuPanel.SetActive(false);
        configPanel.SetActive(true);
    }

    public void OpenCredits() {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMainMenu() {
        shipsPanel.SetActive(false);
        configPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenYouTube() {
        Application.OpenURL("https://www.youtube.com/@TU_CANAL");
    }
}