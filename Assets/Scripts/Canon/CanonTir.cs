using UnityEngine;

public class CanonTir : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Rigidbody bulletPrefabRb;
    [SerializeField] private float bulletSpeed = 10f;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            SpawnBullets(spawnPoints);
        }
    }
    
    void SpawnBullets(GameObject[] spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(bulletPrefabRb, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation) as Rigidbody;
            bulletInstance.AddForce(spawnPoints[i].transform.right * bulletSpeed);
        }
    }
}
