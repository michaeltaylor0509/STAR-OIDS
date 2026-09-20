using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    [SerializeField] private float maxLifeTime = 3f;

    // pública porque Gun.cs necesita asignarla desde fuera al instanciar
    public Vector3 targetVector;

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        // como la bala tiene rotación identity (sin rotar), Space.Self
        // y Space.World coinciden: targetVector se mueve tal cual,
        // sin distorsión ni doble rotación
        transform.Translate(targetVector * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            Destroy(other.gameObject);
    }
}