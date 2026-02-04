using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    [Header("Float Settings")]
    public Transform[] FloatPoints;

    [Header("Forces")]
    public float buoyancyForce = 15f;
    public float waterDrag = 5f;
    public float airDrag = 0.5f;

    [Header("Gizmos")]
    public bool drawGizmos = true;
    public float gizmoPointSize = 0.15f;
    public float gizmoNormalLength = 1.5f;

    Rigidbody rb;
    OceanController ocean;

    // Debug data
    Vector3 avgNormal;
    float[] waterHeights;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        ocean = FindFirstObjectByType<OceanController>();

        waterHeights = new float[FloatPoints.Length];
    }

    void FixedUpdate()
    {
        if (ocean == null || FloatPoints.Length == 0) return;

        avgNormal = Vector3.zero;
        int pointsUnderWater = 0;

        for (int i = 0; i < FloatPoints.Length; i++)
        {
            Transform point = FloatPoints[i];
            Vector3 pos = point.position;

            float waterHeight = ocean.GetWaterHeight(pos);
            waterHeights[i] = waterHeight;

            if (pos.y < waterHeight)
            {
                float depth = waterHeight - pos.y;

                rb.AddForceAtPosition(
                    Vector3.up * depth * buoyancyForce,
                    pos,
                    ForceMode.Force
                );

                avgNormal += ocean.GetWaterNormal(pos);
                pointsUnderWater++;
            }
        }

        if (pointsUnderWater > 0)
        {
            avgNormal.Normalize();

            Quaternion targetRotation =
                Quaternion.FromToRotation(transform.up, avgNormal) * rb.rotation;

            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                Time.fixedDeltaTime * 2f
            );

            rb.linearDamping = waterDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos || FloatPoints == null) return;

        // Float points & water surface
        for (int i = 0; i < FloatPoints.Length; i++)
        {
            Transform point = FloatPoints[i];
            if (point == null) continue;

            // Float point
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point.position, gizmoPointSize);

            if (Application.isPlaying && ocean != null)
            {
                Vector3 waterPos = point.position;
                waterPos.y = waterHeights[i];

                // Water surface sample
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waterPos, Vector3.one * gizmoPointSize);

                // Line to water
                Gizmos.color = Color.white;
                Gizmos.DrawLine(point.position, waterPos);
            }
        }

        // Average normal
        if (Application.isPlaying && avgNormal != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, avgNormal * gizmoNormalLength);
        }
    }
}
