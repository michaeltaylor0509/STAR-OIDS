using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float maxSpeed = 8f;

    private Vector2 thrustDirection;
    private Rigidbody2D rb;

    void Start()
    {
        // rigidbody nos permite aplicar fuerzas en el jugador
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // la rotación se hace aquí (no es física, es solo transform),
        // así queda sincronizada con Gun.cs, que también dispara en Update
        float rotation = Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, -rotation);
    }

    void FixedUpdate()
    {
        // el empuje SÍ va en FixedUpdate porque usa físicas reales (AddForce)
        float thrust = Input.GetAxis("Thrust") * thrustForce;
        thrustDirection = transform.up;
        rb.AddForce(thrust * thrustDirection);

        // limitamos la velocidad máxima para que no acelere sin fin
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}