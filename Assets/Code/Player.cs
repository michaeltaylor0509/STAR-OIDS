using UnityEngine; // indico que uso herramientas de unity como monobehaviour, transform, rigidbody, etc
using System.Collections; // necesario para IEnumerator, el tipo que usan las corrutinas

// creamos script llamado Player y hereda la clase MonoBehaviour que es la base de unity para todos los scripts para los GameObjects y que da aceso a lo típico de update() y demás
public class Player : MonoBehaviour {
    // Esto será el score del player, lo hacemos static para que sea accesible desde cualquier script sin necesidad de tener una referencia al player, y public para que se pueda leer desde otros scripts
    public static long SCORE = 0;

    // El SerializeField permite ver las variables en el inspector
    [SerializeField] private float thrustForce = 2f; // esta va ser la fuerza con la que se empuja al player
    [SerializeField] private float rotationSpeed = 100f; // la velocidad de rotación no muy rápida (es una nava de toneladas)
    [SerializeField] private float maxSpeed = 3f; // velocidad máxima para que la nave no tengo aceleración infinita

    // nuevo: cuánto dura la invulnerabilidad tras perder una vida (en segundos)
    [SerializeField] private float invulnerabilityDuration = 2f;
    // nuevo: cada cuánto alterna visible/invisible durante ese tiempo (velocidad del parpadeo)
    [SerializeField] private float blinkInterval = 0.15f;

    // nuevo: los sprites de las 2 naves posibles, para aplicar el correcto
    // segun lo que se eligiera en el menu de seleccion de nave
    [SerializeField] private Sprite xwingSprite;
    [SerializeField] private Sprite tieSprite;

    // nuevo: en vez de mover un unico Gun con coordenadas, cada nave tiene
    // sus propios objetos de cañon YA colocados en su sitio en la escena;
    // aqui solo activamos/desactivamos los que correspondan
    [SerializeField] private GameObject tieGunObject;        // un solo cañon, para el Tie Fighter
    [SerializeField] private GameObject xwingGunsContainer;  // contenedor con los 2 cañones del X-Wing

    // vector(x,y) que indica dirección de empuje, calculada en cada frame, empieza en 0,0
    private Vector2 thrustDirection;
    // para guardar el componente Rigidbody2D de la nave, que es el que hace que la física funcione y podamos aplicar fuerzas y velocidad
    private Rigidbody2D rb;
    // nuevo: referencia al Sprite Renderer, para poder activarlo/desactivarlo y crear el parpadeo
    private SpriteRenderer sr;
    // nuevo: mientras esté en true, los golpes de asteroide se ignoran por completo
    private bool isInvulnerable = false;

    void Start() {
        SCORE = 0; // se resetea cada vez que la nave nace (incluido tras recargar escena)
        // esta función se llama solo al inicio del juego antes del primer frame del objeto en la escena (antes de que exista), busca el RigidBody dentro del GameObject
        rb = GetComponent<Rigidbody2D>();
        // nuevo: igual que con el rigidbody, buscamos el Sprite Renderer una sola vez al arrancar
        sr = GetComponent<SpriteRenderer>();

        // nuevo: leemos que nave se eligio en el menu (guardada con PlayerPrefs
        // en ShipsMenuController, en la escena MainMenu). El "0" es el valor
        // por defecto si nunca se eligio nada (por si el jugador le da a
        // Jugar sin pasar antes por el menu de naves)
        bool isXWing = PlayerPrefs.GetInt("SelectedShip", 0) == 1;
        // aplicamos el sprite correspondiente a la nave elegida
        sr.sprite = isXWing ? xwingSprite : tieSprite;

        // nuevo: activamos el/los cañon(es) de la nave elegida, y
        // desactivamos los de la otra - cada uno ya esta colocado en su
        // sitio correcto en la escena, no hace falta mover nada por codigo
        tieGunObject.SetActive(!isXWing);
        xwingGunsContainer.SetActive(isXWing);
    }

    void Update() { // este es el bucle que se llama cada frame, es donde se hace la lógica de input y demás
        // Input.GetAxis devuelve un valor entre -1 y 1 dependiendo de la tecla que se pulse, en este caso las flechas o WASD
        // Se pone "Rotate" para saber en que eje necesita
        // mulitplicado por la velocidad que le dimes o por Time.deltaTime para que sea independiente de la velocidad de frames 
        float rotation = Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
        // vector3.forward es el eje Z, el que necesitamos para rotar en 2D, y le decimos que rote en sentido antihorario/negativo o horario/positivo según la tecla pulsada
        // el -rotation está para girar en sentido correcto, el horario pq el de defecto es antihorario
        transform.Rotate(Vector3.forward, -rotation);
    }

    void FixedUpdate() { // este es el bucle que se llama cada frame pero con la frecuencia de la física, es donde se hace la lógica de movimiento y fuerzas
        // lo mismo de antes pero con la fuerza de empuje
        float thrust = Input.GetAxis("Thrust") * thrustForce;
        // te da el valor autmáticamente de apuntar hacia arriba teniendo en cuenta su rotación actual
        thrustDirection = transform.up;
        // addforce en función del rigidbody y unity convierte la fuerza automáticamente en aceleración y velocidad del gameobject
        rb.AddForce(thrust * thrustDirection);
        // esto es para la ponerle un límite a la velocidad máxima
        // linearVelocity.magnitude calcula lonigutd vector velocidad
        if (rb.linearVelocity.magnitude > maxSpeed) {
            // noramlized reeuce cualquier vector a longitud 1 y luego * maxSpeed para velocidad constante
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    // nuevo: lo llama Asteroid.cs (u otro script) cuando la nave choca con algo peligroso
    public void HitByAsteroid() {
        // si ya estamos invulnerables, ignoramos este golpe por completo
        if (isInvulnerable) return;

        // avisamos al gestor de vidas para que reste una
        LivesManager.Instance.LoseLife();
        // arrancamos la corrutina del parpadeo/invulnerabilidad
        StartCoroutine(InvulnerabilityRoutine());
    }

    // nuevo: corrutina = función que se puede "pausar" y seguir más tarde sin bloquear el juego
    private IEnumerator InvulnerabilityRoutine() {
        isInvulnerable = true;
        float elapsed = 0f; // tiempo acumulado dentro de esta corrutina

        // repetimos el parpadeo hasta completar la duración total
        while (elapsed < invulnerabilityDuration) {
            sr.enabled = !sr.enabled; // invierte visible/invisible cada vez
            yield return new WaitForSeconds(blinkInterval); // esperamos antes de seguir
            elapsed += blinkInterval;
        }

        sr.enabled = true; // nos aseguramos de que queda visible al terminar
        isInvulnerable = false;
    }
}