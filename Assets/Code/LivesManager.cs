using UnityEngine; // acceso a MonoBehaviour, SerializeField, etc
using TMPro; // acceso a TMP_Text, el tipo de texto de TextMeshPro

public class LivesManager : MonoBehaviour {
    // static: así cualquier script llama a LivesManager.Instance sin buscar una referencia
    public static LivesManager Instance;

    [SerializeField] private int maxLives = 5; // tope máximo, para cuando metas power-up de vida
    [SerializeField] private int startingLives = 3; // con cuántas vidas empieza la partida
    [SerializeField] private TMP_Text livesText; // el texto del HUD que muestra las vidas

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip loseLifeSound;

    private int currentLives; // las vidas que quedan ahora mismo

    void Awake() {
        // Awake se ejecuta ANTES que Start de cualquier otro script,
        // así la instancia ya está lista cuando Player.cs la necesite
        Instance = this;
    }

    void Start() {
        currentLives = startingLives;
        UpdateLivesText();
    }

    // lo llama Player.cs cuando un golpe de asteroide cuenta de verdad
    public void LoseLife() {
        currentLives--;
        UpdateLivesText();

        audioSource.PlayOneShot(loseLifeSound);

        // si llegamos a 0 (o menos), es Game Over real
        if (currentLives <= 0) {
            GameOverManager.Instance.ShowGameOver();
        }
    }

    // preparado para cuando metas el power-up de vida extra
    public void AddLife() {
        if (currentLives < maxLives) {
            currentLives++;
            UpdateLivesText();
        }
    }

    private void UpdateLivesText() {
        livesText.text = "Lives: " + currentLives;
    }
}