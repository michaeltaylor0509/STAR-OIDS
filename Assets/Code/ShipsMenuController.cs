using UnityEngine;

public class ShipsMenuController : MonoBehaviour {
    // solo necesitamos los 2 marcos de resaltado, nada mas
    [SerializeField] private GameObject xwingHighlight;
    [SerializeField] private GameObject tieHighlight;

    private bool isXWing;

    // al abrir el panel, mostramos resaltada la nave que ya estaba
    // guardada como elegida la ultima vez
    void OnEnable() {
        isXWing = PlayerPrefs.GetInt("SelectedShip", 0) == 1;
        UpdateHighlight();
    }

    // esto lo llama el boton/imagen del X-Wing directamente
    public void SelectXWing() {
        isXWing = true;
        SaveAndUpdate();
    }

    // esto lo llama el boton/imagen del Tie Fighter directamente
    public void SelectTie() {
        isXWing = false;
        SaveAndUpdate();
    }

    private void SaveAndUpdate() {
        // guardamos la eleccion en el disco, para que Player.cs (en otra
        // escena) pueda leerla despues, y se recuerde entre partidas
        PlayerPrefs.SetInt("SelectedShip", isXWing ? 1 : 0);
        PlayerPrefs.Save();

        UpdateHighlight();
    }

    private void UpdateHighlight() {
        // activamos el resaltado SOLO en la nave elegida, apagamos el otro
        xwingHighlight.SetActive(isXWing);
        tieHighlight.SetActive(!isXWing);
    }
}