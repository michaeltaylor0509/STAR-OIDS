using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // instanciamos con rotación IDENTITY (sin rotación propia),
            // igual que hace el profesor: así evitamos que la bala
            // rote el vector de movimiento dos veces
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // le pasamos la dirección ya calculada en espacio del mundo
            // (transform.up del Gun = misma orientación que la nave, porque
            // Gun es hijo de Ship y hereda su rotación)
            bullet.GetComponent<Bullet>().targetVector = transform.up;
        }
    }
}