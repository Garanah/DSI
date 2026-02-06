using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatBuoyancy : MonoBehaviour
{
    public Ocean ocean;
    public Transform[] floatPoints;

    [Header("Buoyancy")]
    public float buoyancy = 25f;
    public float maxSubmergeDepth = 1.0f;
    public float normalSampleRadius = 0.7f;

    [Header("Drag in water")]
    public float waterLinearDamping = 2f;
    public float waterAngularDamping = 1f;

    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (!ocean || floatPoints == null || floatPoints.Length == 0) return;

        float t = Time.time;
        int underwater = 0;
        Vector3 avgNormal = Vector3.zero;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            Vector3 p = floatPoints[i].position;
            float waterY = ocean.GetHeight(p, t);
            float depth = waterY - p.y;

            if (depth > 0f)
            {
                underwater++;

                float sub01 = Mathf.Clamp01(depth / Mathf.Max(0.001f, maxSubmergeDepth));
                Vector3 n = ocean.GetNormal(p, t, normalSampleRadius);
                avgNormal += n;

                rb.AddForceAtPosition(n * (buoyancy * sub01), p, ForceMode.Acceleration);
            }
        }

        if (underwater > 0)
        {
            rb.linearDamping = waterLinearDamping;
            rb.angularDamping = waterAngularDamping;

            Vector3 up = (avgNormal / underwater).normalized;
            rb.rotation = Quaternion.FromToRotation(transform.up, up) * rb.rotation;
        }
        else
        {
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
        
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void OnDrawGizmos()
    {
        if (floatPoints == null) return;
        
        Gizmos.color = Color.green;
        for (int i = 0; i < floatPoints.Length; i++)
        {
            if (floatPoints[i] == null) continue;
            Gizmos.DrawSphere(floatPoints[i].position, 0.08f);

            if (Application.isPlaying && ocean != null)
            {
                float waterY = ocean.GetHeight(floatPoints[i].position, Time.time);
                Vector3 waterPoint = new Vector3(floatPoints[i].position.x, waterY, floatPoints[i].position.z);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waterPoint, 0.06f);
                
                Vector3 n = ocean.GetNormal(floatPoints[i].position, Time.time, normalSampleRadius);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(waterPoint, n * 0.6f);

                Gizmos.color = Color.green;
            }
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.15f);
    }
}
