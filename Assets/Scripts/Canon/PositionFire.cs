using UnityEngine;
using UnityEngine.Serialization;

public class PositionFire : MonoBehaviour
{
    
    [Header("LimitMax")]
    [SerializeField] Vector3 leftUp;
    [SerializeField] Vector3 leftDown;
    [SerializeField] Vector3 rightUp;
    [SerializeField] Vector3 rightDown;
    
    [Header("positions")]
    [SerializeField] Vector3 startPosition;
    private Vector3 actualPosition;

    [Header("Movement")] 
    [SerializeField] private float moveSpeed;
    float horizontal;
    float vertical;
    public static bool shooting;

    [SerializeField] private Rigidbody canonMarkerRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        manageInputs();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (shooting)
        {
            Debug.Log("feur");
            MoveFirePoint();
        }
    }

    private void manageInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        shooting = Input.GetKey(KeyCode.JoystickButton5);
    }

    void CheckXPosition()
    {
        
    }

    void CheckZPosition()
    {
        
    }

    void LimitMaker()
    {
        
    }

    void MoveFirePoint()
    {
        float maxSpeed = moveSpeed + 0.1f;
        if (canonMarkerRb.linearVelocity.magnitude < maxSpeed)
        {
            canonMarkerRb.AddForce(transform.forward * (vertical * moveSpeed));
        }
    }
}
