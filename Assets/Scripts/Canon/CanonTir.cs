using UnityEngine;

public class CanonTir : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Rigidbody bulletPrefabRb;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private GameObject previewShotMesh;
    public bool right;

    private bool preview = false;
    private bool shoot = false;

    void InputManager()
    {
        if (right)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                preview = !preview;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button5))
            {
                shoot = !shoot;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                preview = !preview;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button4))
            {
                shoot = !shoot;
            }
        }
        
    }

    void Update()
    {
        InputManager();
    }
    void FixedUpdate()
    {
        if (preview)
        {
            previewShotMesh.SetActive(true);
            preview = !preview;
        }
        if (shoot)
        {   
            previewShotMesh.SetActive(false);
            SpawnBullets(spawnPoints);
            shoot = !shoot;
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
