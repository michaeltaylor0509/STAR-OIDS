using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float spawnRatePerMinute = 30f;
    [SerializeField] private float spawnRateIncrement = 1f;
    [SerializeField] private float spawnMargin = 1f; // cuanto fuera de pantalla nace

    private float spawnNext = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Time.time > spawnNext)
        {
            spawnNext = Time.time + 60 / spawnRatePerMinute;
            spawnRatePerMinute += spawnRateIncrement;

            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        // calculamos el área visible actual de la cámara (cambia porque la cámara se mueve)
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector2 camCenter = cam.transform.position;

        // elegimos un borde aleatorio: 0=arriba, 1=abajo, 2=izquierda, 3=derecha
        int edge = Random.Range(0, 4);
        Vector2 spawnPos;

        switch (edge)
        {
            case 0:
                spawnPos = new Vector2(Random.Range(-halfWidth, halfWidth), halfHeight + spawnMargin);
                break;
            case 1:
                spawnPos = new Vector2(Random.Range(-halfWidth, halfWidth), -halfHeight - spawnMargin);
                break;
            case 2:
                spawnPos = new Vector2(-halfWidth - spawnMargin, Random.Range(-halfHeight, halfHeight));
                break;
            default:
                spawnPos = new Vector2(halfWidth + spawnMargin, Random.Range(-halfHeight, halfHeight));
                break;
        }

        // el punto calculado es relativo al centro (0,0); lo desplazamos a donde esté la cámara ahora
        spawnPos += camCenter;

        // en vez de Instantiate, le pedimos un asteroide grande al manager
        GameObject asteroid = AsteroidPoolManager.Instance.GetBig(spawnPos);

        // dirección hacia el centro de la cámara, para que el asteroide atraviese la pantalla
        Vector2 direction = (camCenter - spawnPos).normalized;
        asteroid.GetComponent<Asteroid>().SetDirection(direction);
    }
}