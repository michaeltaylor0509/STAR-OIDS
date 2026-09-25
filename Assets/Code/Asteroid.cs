using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxLifeTime = 15f;
    [SerializeField] private int scoreValue = 50;

    // AQUI ESTA LO NUEVO: un enum con 3 opciones posibles.
    // En vez de arrastrar un prefab o escribir texto a mano, en el
    // Inspector veras un DESPLEGABLE con estas 3 opciones para elegir
    public enum NextSize { None, Mid, Small }
    [SerializeField] private NextSize nextSize = NextSize.None; // None = no se rompe mas

    [SerializeField] private int fragmentCount = 2;

    [SerializeField] private AudioClip destroySound;

    private Vector2 direction = Vector2.down;
    private Collider2D col;

    // OnEnable en vez de Start, porque este asteroide se va a reutilizar
    // muchas veces (pooling), y necesita "resetearse" cada vez que despierta
    void OnEnable() {
        col = GetComponent<Collider2D>();
        col.enabled = true;
        // programamos su "muerte reciclada" pasado maxLifeTime segundos
        Invoke(nameof(ReturnToPool), maxLifeTime);
    }

    public void SetDirection(Vector2 dir) {
        direction = dir;
    }

    void Update() {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // lo llama Bullet.cs en el momento exacto del impacto
    public void Fragment(Vector2 impactDirection) {
        col.enabled = false;
        ScoreManager.AddScore(scoreValue);

            AudioSource.PlayClipAtPoint(destroySound, transform.position);

        // si nextSize NO es None, significa que tenemos que generar fragmentos
        if (nextSize != NextSize.None) {
            SpawnFragments(impactDirection);
        }

        ReturnToPool(); // en vez de Destroy(gameObject)
    }

    private void SpawnFragments(Vector2 impactDirection) {
        float angleStep = 90f;
        float startAngle = -((fragmentCount - 1) * angleStep) / 2f;

        for (int i = 0; i < fragmentCount; i++) {
            float angle = startAngle + i * angleStep;
            Vector2 fragmentDir = Rotate(impactDirection, angle);

            // segun el enum, le pedimos al manager el tamaño que corresponda
            GameObject fragment;
            if (nextSize == NextSize.Mid) {
                fragment = AsteroidPoolManager.Instance.GetMid(transform.position);
            } else { // solo puede ser Small en este punto
                fragment = AsteroidPoolManager.Instance.GetSmall(transform.position);
            }

            // le decimos al fragmento recien despertado hacia donde moverse
            fragment.GetComponent<Asteroid>().SetDirection(fragmentDir);
        }
    }

    // formula de rotacion 2D, igual que siempre, sin cambios
    private Vector2 Rotate(Vector2 v, float degrees) {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<Player>().HitByAsteroid();
        }
    }

    private void ReturnToPool() {
        CancelInvoke(nameof(ReturnToPool));
        AsteroidPoolManager.Instance.ReturnObject(gameObject);
    }
}