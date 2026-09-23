using UnityEngine;

// cuidado con el collider de la bala que tuve problemas con que se quitaba al inicio por collider con el player
// cuidado con poner dos guns cerca porque los collider de las balas destruyen entre sí mejor hacer un sprite de bala doble
// cuidado con duplicar el arma para diaparar doble porque también se duplica la puntuación obtenida
//
// CAMBIO A OBJECT POOLING: en vez de crear una bala nueva con Instantiate() y
// destruirla con Destroy() cada vez, ahora la misma bala se reutiliza una y
// otra vez durante toda la partida, solo activandose/desactivandose. Dejo las
// lineas viejas comentadas al lado de las nuevas para ver el cambio exacto
public class Bullet : MonoBehaviour {
    [SerializeField] private int speed = 10; // velocidad de la bala
    [SerializeField] private float maxLifeTime = 0.7f; // el tiempo maximo de bala
    public Vector3 targetVector; // la direccion en la que se mueve la bala, asignada desde Gun.cs
    private Collider2D col; // componente collider de la bala, para desactivarlo al impactar

    // NUEVO: guardamos a que pool pertenece esta bala concreta, para poder
    // devolversela cuando termine su trabajo (en vez de destruirla)
    private ObjectPool pool;

    // ANTES: void Start() { ... } — Start() solo se ejecuta UNA VEZ en toda
    // la vida del objeto (la primera vez que existe)
    //
    // AHORA: void OnEnable() { ... } — se ejecuta CADA VEZ que el objeto se
    // activa con SetActive(true). Como la misma bala se reutiliza muchas
    // veces, necesitamos "resetearla" cada vez que vuelve a despertarse,
    // no solo la primera vez
    void OnEnable() {
        // guardamos el collider para desactivarlo al impactar
        col = GetComponent<Collider2D>();
        // por si se habia quedado desactivado del impacto anterior, lo reactivamos
        col.enabled = true;

        // ANTES: Destroy(gameObject, maxLifeTime);
        // destruia la bala de verdad pasado ese tiempo si no impactaba nada
        //
        // AHORA: Invoke funciona parecido (llama a algo pasado un tiempo),
        // pero en vez de destruir, llamamos a ReturnToPool() para dormirla
        Invoke(nameof(ReturnToPool), maxLifeTime);
    }

    // NUEVO: Gun.cs nos dice a que pool pertenecemos justo al pedirnos (GetObject)
    public void SetPool(ObjectPool sourcePool) {
        pool = sourcePool;
    }

    void Update() {
        // movemos la bala en la direccion asignada, igual que antes, sin cambios
        transform.Translate(targetVector * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // esto sigue igual: si la bala impacta con la nave, no cuenta ni se destruye
        if (collider.gameObject.CompareTag("Player")) {
            return;
        }

        // desactivamos el collider con enabled, igual que antes, misma razon:
        // evitar contar puntos multiples veces si impacta varias cosas el mismo frame
        col.enabled = false;

        if (collider.gameObject.CompareTag("Asteroid")) {
            collider.GetComponent<Asteroid>().Fragment(targetVector);
        }
        /* else if (collider.gameObject.CompareTag("Enemy")) {
            collider.GetComponent<Enemy>().DestroyEnemy();
        }
        else if (collider.gameObject.CompareTag("EnemyBullet")) {
            Destroy(collider.gameObject);
        }
        */

        // ANTES: Destroy(gameObject); // borraba la bala tras impactar
        // AHORA: la devolvemos al pool en vez de destruirla de verdad
        ReturnToPool();
    }

    // NUEVO: funcion central que sustituye a todos los Destroy(gameObject) de arriba
    private void ReturnToPool() {
        // CancelInvoke cancela el Invoke que programamos en OnEnable, por si
        // la bala choco ANTES de cumplirse maxLifeTime. Sin esto, se
        // intentaria devolver la bala DOS veces (una por el choque, otra
        // por el tiempo maximo), lo cual daria error
        CancelInvoke(nameof(ReturnToPool));
        pool.ReturnObject(gameObject);
    }
}