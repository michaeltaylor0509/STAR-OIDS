using UnityEngine;

// NOTA: el Rigidbody2D del Player debe tener Freeze Rotation Z activado
// (Constraints), o cada choque contra este borde deja una rotacion residual
public class MapBorder : MonoBehaviour {
    // OnCollisionEnter2D (no OnTriggerEnter2D) porque el Tilemap Collider 2D
    // NO tiene Is Trigger activado: queremos colision fisica real,
    // para que la nave rebote y nunca pueda atravesar el borde del mapa
    private void OnCollisionEnter2D(Collision2D collision) {
        // comprobamos si lo que ha chocado es la nave del jugador
        if (collision.gameObject.CompareTag("Player")) {
            // le avisamos a la nave, ella decide si el golpe cuenta
            // (por ejemplo, lo ignora si ya esta invulnerable)
            collision.gameObject.GetComponent<Player>().HitByAsteroid();
        }
    }
}