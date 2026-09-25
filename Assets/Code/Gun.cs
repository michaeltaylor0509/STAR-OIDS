using UnityEngine;

// CAMBIO A OBJECT POOLING: en vez de tener una referencia directa al prefab
// de la bala, ahora tenemos una referencia al POOL que las gestiona
public class Gun : MonoBehaviour {
    // ANTES: [SerializeField] private GameObject bulletPrefab; // referencia al prefab de la bala
    // AHORA: referencia al pool en vez del prefab directo
    [SerializeField] private ObjectPool bulletPool;

    [SerializeField] private float fireCooldown = 0.5f; // tiempo minimo entre disparos para no spamear balas
    
    // referencia al AudioSource de este cañon, y el sonido a reproducir
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    // esta es necesaria para el cooldown, guarda info entre frames al estar fuera de la función Update, y es privada para saber si ha pasado tiempo o no
    // nextFireTime = 0;
    // disparo en el segundo 2.0 y como 2.0 >= 0 es true disparamos y nextFireTime = 2.0 + 0.3 = 2.3
    // disparo en el segundo 2.1 y como 2.1 >= 2.3 es false no disparamos hasta 2.3 nada
    private float nextFireTime = 0f;

    void Update() {
        // Fire1 es el input configurado en Project Settings
        // a diferencia de GetButtonDown que da true una vez sin importar cuanto tiempo presiones la tecla, GetButton da true mientras la tecla esté presionada
        // Time.time es el tiempo desde que empezó el juego, en segundos, y lo usamos para saber si ha pasado el tiempo suficiente desde el último disparo
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) {
            nextFireTime = Time.time + fireCooldown;

            // ANTES:
            // GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            // creaba una bala NUEVA cada vez, con Instantiate
            //
            // AHORA: pedimos una bala YA EXISTENTE al pool, ya colocada en
            // la posicion y rotacion correctas (el pool se encarga de eso)
            GameObject bullet = bulletPool.GetObject(transform.position, transform.rotation);

            // NUEVO: le decimos a esta bala concreta a que pool pertenece,
            // para que sepa donde devolverse cuando termine
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetPool(bulletPool);

            // tras crear/obtener la copia, le decimos hacia donde va la bala (hacia donde apunta el player)
            bulletScript.targetVector = transform.up;
            // importante para arreglar el error de disparo con bugs visuales como antes con Quaternion.identity, que es la rotación por defecto (0,0,0) y no la del player, que es la que queremos para que la bala salga hacia donde apunta el player
        
            // reproduce el sonido de disparo cada vez que se dispara
            audioSource.PlayOneShot(shootSound);
        }
    }
}