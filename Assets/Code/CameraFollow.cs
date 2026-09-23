using UnityEngine;
// cuidado con el bug de que solo se muestra skybox porque se estaba teniendo z=0 que clippeaba porque la distancia minima para ver algo estaba confiurada en 0.3 entonces no se veia nada, por eso 
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // referencia a la nave, que es el objetivo a seguir
    [SerializeField] private float zDistance = -10f; // distancia fija en Z, siempre correcta

    private Vector3 offset; // distancia que debe mantener la cámara respecto a la nave, calculada al inicio y luego usada en LateUpdate

    void Start() {
        // guardamos solo el offset en X e Y (normalmente 0,0 si la cámara
        // arranca centrada en la nave), pero forzamos Z manualmente
        offset = new Vector3(
            transform.position.x - target.position.x,
            transform.position.y - target.position.y,
            zDistance
        );
    }

    // este bucle se ejecuta después de que todos los Update() y FixedUpdate() hayan terminado, y es el momento correcto para mover la cámara
    void LateUpdate() {
        transform.position = target.position + offset;
    }
}