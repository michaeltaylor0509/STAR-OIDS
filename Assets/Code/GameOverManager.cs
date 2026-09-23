using UnityEngine;
using UnityEngine.SceneManagement; // necesario para SceneManager.LoadScene()
using TMPro;

public class GameOverManager : MonoBehaviour {

    // static: cualquier script llama a GameOverManager.Instance.ShowGameOver()
    // sin necesitar arrastrar una referencia a mano
    public static GameOverManager Instance;

    [SerializeField] private GameObject gameOverPanel; // el panel a mostrar
    [SerializeField] private TMP_Text finalScoreText;   // texto de puntuación final
    [SerializeField] private TMP_Text maxStreakText;    // texto de racha máxima

    void Awake() {
        // Awake se ejecuta ANTES que Start de cualquier otro script,
        // así la instancia ya está lista cuando otros scripts la necesiten
        Instance = this;
    }

    public void ShowGameOver() {
        gameOverPanel.SetActive(true); // hacemos visible el panel

        // "texto" + variable -> concatena y muestra el valor actual
        finalScoreText.text = "Puntuación: " + Player.SCORE;
        maxStreakText.text = "x" + ScoreMultiplier.MaxMultiplierReached;

        Time.timeScale = 0f; // congelamos el juego entero
    }

    public void Restart() {
        // ojo al orden: primero descongelamos, LUEGO recargamos.
        // si recargas con timeScale en 0, la escena nueva nace congelada
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // nuevo: lleva al jugador de vuelta al Menu Inicio desde la pantalla de Game Over
    public void GoToMainMenu() {
        Time.timeScale = 1f; // descongelamos, igual que en Restart()
        SceneManager.LoadScene("MainMenu");
    }
}