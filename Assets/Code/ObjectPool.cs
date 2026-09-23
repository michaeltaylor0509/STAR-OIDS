using UnityEngine;
using System.Collections.Generic; // necesario para poder usar List<>

public class ObjectPool : MonoBehaviour {
    // el prefab que vamos a clonar de antemano (Bullet, AsteroidLarge, etc)
    [SerializeField] private GameObject prefab;
    // cuantas copias creamos al arrancar, ajustable segun cuantas necesites a la vez
    [SerializeField] private int poolSize = 20;

    // lista donde guardamos TODOS los objetos creados, esten activos o dormidos
    private List<GameObject> pool = new List<GameObject>();

    void Start() {
        // creamos todas las copias de golpe al arrancar, y las dejamos dormidas
        for (int i = 0; i < poolSize; i++) {
            GameObject obj = Instantiate(prefab);

            // le asignamos el pool AQUI, justo despues de crearla y ANTES de
            // dormirla, para que ya tenga la referencia lista desde el primer
            // instante (Bullet.cs necesita saber a que pool pertenece nada
            // mas nacer, ya que su OnEnable programa un Invoke de reciclaje)
            Bullet bulletScript = obj.GetComponent<Bullet>();
            if (bulletScript != null) { // por si este pool se usa con otro prefab que no sea Bullet
                bulletScript.SetPool(this);
            }

            obj.SetActive(false); // dormida, lista para usarse mas tarde
            pool.Add(obj); // la metemos en la lista para poder encontrarla despues
        }
    }

    // esto es lo que se llama EN VEZ de Instantiate() normal
    public GameObject GetObject(Vector3 position, Quaternion rotation) {
        // recorremos la lista buscando la primera que este dormida
        foreach (GameObject obj in pool) {
            if (!obj.activeInHierarchy) { // "!" significa NO, o sea "si NO esta activa"
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true); // la despertamos
                return obj; // la devolvemos para que quien la pidio la use
            }
        }

        // si llegamos aqui, es que TODAS estaban despiertas a la vez (caso raro)
        // creamos una extra de emergencia y la sumamos al pool para el futuro
        GameObject newObj = Instantiate(prefab, position, rotation);

        Bullet newBulletScript = newObj.GetComponent<Bullet>();
        if (newBulletScript != null) {
            newBulletScript.SetPool(this);
        }

        pool.Add(newObj);
        return newObj;
    }

    // esto es lo que se llama EN VEZ de Destroy()
    public void ReturnObject(GameObject obj) {
        obj.SetActive(false); // solo la dormimos, nunca se destruye de verdad
    }
}