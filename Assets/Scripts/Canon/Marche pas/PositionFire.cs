using UnityEngine;
using UnityEngine.Serialization;

public class PositionFire : MonoBehaviour
{
    
    [Header("LimitMax")]
    [SerializeField] GameObject leftUp;
    [SerializeField] GameObject leftDown;
    [SerializeField] GameObject rightUp;
    [SerializeField] GameObject rightDown;
    [SerializeField] Vector3[] zone =  new Vector3[4];
    
    [Header("positions")]
    [SerializeField] GameObject startPosition;
    private Vector3 actualPosition;

    [Header("Movement")] 
    [SerializeField] private float moveSpeed;
    float horizontal;
    float vertical;
    public static bool shooting;

    [SerializeField] private GameObject canonMarker;
    void Start()
    {
        LimitMaker();
    }

    private void Update()
    {
        manageInputs();
        LimitMaker();
    }
    void FixedUpdate()
    {
        if (shooting)
        {
            MoveFirePoint();
        }
    }

    private void manageInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        shooting = Input.GetKey(KeyCode.JoystickButton5);
    }
    
    bool IsPointInside(Vector3 point, Vector3[] polygon)
    {
        Vector2 p = new Vector2(point.x, point.z);
        bool inside = false;

        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            Vector2 pi = new Vector2(polygon[i].x, polygon[i].z);
            Vector2 pj = new Vector2(polygon[j].x, polygon[j].z);

            if ((pi.y > p.y) != (pj.y > p.y) &&
                (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    
    Vector2 To2D(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    Vector3 ClosestPoint(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        float t = Vector3.Dot(p -a, ab) / Vector3.Dot(ab, ab);
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }

    Vector3 ClampPointToZone(Vector3 point, Vector3[] polygon)
    {
        Vector3 closestPoint = polygon[0];
        float minDistance = float.MaxValue;

        for (int i = 0; i < polygon.Length; i++)
        {
            Vector3 a = polygon[i];
            Vector3 b = polygon[(i+1)%polygon.Length];
            
            Vector3 candidate = ClosestPoint(a, b, point);
            float dist = Vector3.SqrMagnitude(point - candidate);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestPoint = candidate;
            }
        }
        
        return closestPoint;
    }
    
    void LimitMaker()
    {
        zone[0] = leftUp.transform.position;
        zone[1] = rightUp.transform.position;
        zone[2] = rightDown.transform.position;
        zone[3] = leftDown.transform.position;
    }

    void MoveFirePoint()
    {
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * (moveSpeed * Time.deltaTime);
        canonMarker.transform.position += movement;
    
        Vector3 desiredPosition = canonMarker.transform.position;

        if (IsPointInside(desiredPosition, zone))
        {
            canonMarker.transform.position = desiredPosition;
        }
        else
        {
            canonMarker.transform.position = ClampPointToZone(desiredPosition, zone);
        }
    }
    void OnDrawGizmos()
    {
        if (zone == null || zone.Length < 2) return;
        
        Gizmos.color = Color.green;
        for (int i = 0; i < zone.Length; i++)
        {
            Gizmos.DrawLine(zone[i], zone[(i+1)%zone.Length]);
        }
    }
}
