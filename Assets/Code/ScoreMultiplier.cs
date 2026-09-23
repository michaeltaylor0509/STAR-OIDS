using UnityEngine; // acceso a MonoBehaviour, Time, Mathf, SerializeField...
using TMPro;        // acceso a TMP_Text (el tipo de texto de TextMeshPro)

public class ScoreMultiplier : MonoBehaviour {

    // cuánto se multiplica el valor cada vez (x2 cada intervalo)
    [SerializeField] private float growthFactor = 2f;

    // cada cuántos segundos sube el multiplicador (60 = cada minuto)
    [SerializeField] private float minuteInterval = 60f;

    // referencia al texto del HUD donde se muestra "x2", "x4"...
    [SerializeField] private TMP_Text multiplierText;

    // static: solo existe UN multiplicador compartido por todo el juego,
    // así cualquier script puede leerlo escribiendo ScoreMultiplier.CurrentMultiplier
    public static float CurrentMultiplier = 1f;

    // guarda el valor más alto alcanzado, para mostrarlo en el Game Over
    public static int MaxMultiplierReached = 1;

    // contador de tiempo acumulado, no reseteado por nadie más que este script
    private float timer = 0f;

    void Start() {
        // reseteamos al arrancar la escena (incluye cuando se recarga tras Restart),
        // porque al ser 'static' NO se resetearían solos entre partidas
        CurrentMultiplier = 1f;
        MaxMultiplierReached = 1;
        UpdateText();
    }

    void Update() {
        // sumamos el tiempo transcurrido este frame al contador
        timer += Time.deltaTime;

        // si ya ha pasado un minuto completo desde la última subida...
        if (timer >= minuteInterval) {
            timer = 0f; // reiniciamos el contador para el siguiente minuto

            // multiplicamos el valor actual por 2 (1 -> 2 -> 4 -> 8 -> 16...)
            CurrentMultiplier *= growthFactor;

            // Mathf.RoundToInt redondea el decimal al entero más cercano,
            // solo para mostrarlo bonito en pantalla (ej. "x4")
            int rounded = Mathf.RoundToInt(CurrentMultiplier);

            // solo guardamos el récord si el nuevo valor es mayor que el anterior
            if (rounded > MaxMultiplierReached) {
                MaxMultiplierReached = rounded;
            }

            UpdateText(); // refrescamos el HUD con el nuevo número
        }
    }

    private void UpdateText() {
        // "x" + número -> concatenamos texto y número en un solo string
        multiplierText.text = "x" + Mathf.RoundToInt(CurrentMultiplier);
    }
}