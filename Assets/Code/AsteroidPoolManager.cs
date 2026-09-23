using UnityEngine;
using System.Collections.Generic; // esto nos da acceso a List<>

public class AsteroidPoolManager : MonoBehaviour {
    // static: solo existe UN manager en todo el juego, cualquier script
    // accede escribiendo AsteroidPoolManager.Instance, sin buscar nada
    public static AsteroidPoolManager Instance;

    // los 3 prefabs que vamos a reciclar, uno por tamaño
    [SerializeField] private GameObject BigPrefab;
    [SerializeField] private GameObject MidPrefab;
    [SerializeField] private GameObject smallPrefab;

    // cuantas copias de cada tamaño creamos de antemano
    [SerializeField] private int BigCount = 5;
    [SerializeField] private int MidCount = 10;
    [SerializeField] private int smallCount = 20;

    // 3 listas separadas, una para guardar cada tamaño de asteroide
    private List<GameObject> BigPool = new List<GameObject>();
    private List<GameObject> MidPool = new List<GameObject>();
    private List<GameObject> smallPool = new List<GameObject>();

    void Awake() {
        // nos guardamos a nosotros mismos en la variable static, ANTES
        // de que cualquier otro script (como Asteroid) intente usarnos
        Instance = this;
    }

    void Start() {
        // rellenamos las 3 listas de golpe al arrancar el juego
        FillPool(BigPool, BigPrefab, BigCount);
        FillPool(MidPool, MidPrefab, MidCount);
        FillPool(smallPool, smallPrefab, smallCount);
    }

    // funcion de ayuda: crea "count" copias dormidas de un prefab,
    // y las va metiendo en la lista que le pasemos
    private void FillPool(List<GameObject> list, GameObject prefab, int count) {
        for (int i = 0; i < count; i++) {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // dormida desde el principio
            list.Add(obj); // la metemos en la lista
        }
    }

    // funcion de ayuda: busca una copia dormida DENTRO de una lista
    // concreta, la despierta, y la devuelve
    private GameObject GetFromList(List<GameObject> list, GameObject prefab, Vector3 pos) {
        // foreach = "para cada GameObject dentro de esta lista..."
        foreach (GameObject obj in list) {
            if (!obj.activeInHierarchy) { // si NO esta activa ahora mismo
                obj.transform.position = pos;
                obj.SetActive(true);
                return obj; // la encontramos, la devolvemos y paramos aqui
            }
        }

        // si el foreach termino sin encontrar ninguna libre (todas ocupadas),
        // creamos una extra de emergencia
        GameObject newObj = Instantiate(prefab, pos, Quaternion.identity);
        list.Add(newObj);
        return newObj;
    }

    // estas 3 funciones son las que se llaman DESDE FUERA (desde Asteroid.cs
    // o EnemySpawner.cs). Cada una simplemente usa la lista que le corresponde
    public GameObject GetBig(Vector3 pos) {
        return GetFromList(BigPool, BigPrefab, pos);
    }

    public GameObject GetMid(Vector3 pos) {
        return GetFromList(MidPool, MidPrefab, pos);
    }

    public GameObject GetSmall(Vector3 pos) {
        return GetFromList(smallPool, smallPrefab, pos);
    }

    // esta la usan TODOS los tamaños por igual, para devolver un asteroide
    // al pool (dormirlo) cuando ya no se necesita
    public void ReturnObject(GameObject obj) {
        obj.SetActive(false);
    }
}