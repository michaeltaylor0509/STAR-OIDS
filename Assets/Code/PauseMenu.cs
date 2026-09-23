using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pausePanel; // panel que se muestra al pausar
    [SerializeField] private GameObject continueButton; // botón seleccionado por defecto

    private bool isPaused = false;

    void Update() {
        // ESC alterna entre pausar y reanudar
        // (quitado el atajo de debug de KeyCode.G que forzaba Game Over)
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause() {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // congela el juego

        // así las flechas ya funcionan sin tener que hacer clic antes
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    public void Resume() {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // vuelve a correr el tiempo normal
    }

    public void Restart() {
        Time.timeScale = 1f; // importante: si no, la escena nueva nace pausada
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [SerializeField] private GameObject configPanel; // referencia al ConfigPanel LOCAL de esta escena

    public void OpenConfig() {
        pausePanel.SetActive(false);
        configPanel.SetActive(true);
        // Time.timeScale sigue en 0, la partida sigue congelada de verdad,
        // no destruida - al volver, todo sigue exactamente donde lo dejaste
    }

    public void CloseConfig() {
        configPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // ANTES: ExitGame() llamaba a Application.Quit() con un TODO sin resolver
    // AHORA: mismo patrón que GameOverManager.GoToMainMenu() — descongelar
    // primero, luego cargar la escena del menú principal
    public void GoToMainMenu() {
        Time.timeScale = 1f; // descongelamos, si no la escena de menú nacería pausada
        SceneManager.LoadScene("MainMenu");
    }
}