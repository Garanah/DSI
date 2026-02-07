using Ditzelgames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    // public properties
    public float AirDrag = 1f;
    public float WaterDrag = 10f;
    public bool AffectDirection = true;
    public bool AttachToSurface = false;
    public Transform[] FloatPoints;

    // used components
    protected Rigidbody Rigidbody;
    protected Ocean Ocean;

    // water line
    protected float WaterLine;
    protected Vector3[] WaterLinePoints;

    // help vectors
    protected Vector3 smoothVectorRotation;
    protected Vector3 TargetUp;
    protected Vector3 centerOffset;

    public Vector3 Center => transform.position + centerOffset;

    void Awake()
    {
        Ocean = FindFirstObjectByType<Ocean>();
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;

        // compute center
        WaterLinePoints = new Vector3[FloatPoints.Length];
        for (int i = 0; i < FloatPoints.Length; i++)
            WaterLinePoints[i] = FloatPoints[i].position;

        centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position;
    }

    void FixedUpdate()
    {
        float newWaterLine = 0f;
        bool pointUnderWater = false;

        // sample water height at float points
        for (int i = 0; i < FloatPoints.Length; i++)
        {
            WaterLinePoints[i] = FloatPoints[i].position;

            float h = Ocean.GetHeight(WaterLinePoints[i], Time.time);
            WaterLinePoints[i].y = h;

            newWaterLine += h / FloatPoints.Length;

            if (h > FloatPoints[i].position.y)
                pointUnderWater = true;
        }

        float waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        // get surface normal from ocean
        TargetUp = Ocean.GetNormal(Center, Time.time);

        // gravity & drag
        Vector3 gravity = Physics.gravity;
        Rigidbody.linearDamping = AirDrag;

        if (WaterLine > Center.y)
        {
            Rigidbody.linearDamping = WaterDrag;

            if (AttachToSurface)
            {
                Rigidbody.position = new Vector3(
                    Rigidbody.position.x,
                    WaterLine - centerOffset.y,
                    Rigidbody.position.z
                );
            }
            else
            {
                gravity = AffectDirection
                    ? TargetUp * -Physics.gravity.y
                    : -Physics.gravity;

                transform.Translate(Vector3.up * waterLineDelta * 0.9f, Space.World);
            }
        }

        Rigidbody.AddForce(
            gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y), 0f, 1f),
            ForceMode.Acceleration
        );

        // rotation
        if (pointUnderWater)
        {
            TargetUp = Vector3.SmoothDamp(
                transform.up,
                TargetUp,
                ref smoothVectorRotation,
                0.2f
            );

            Rigidbody.rotation =
                Quaternion.FromToRotation(transform.up, TargetUp) * Rigidbody.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        if (FloatPoints == null) return;

        for (int i = 0; i < FloatPoints.Length; i++)
        {
            if (FloatPoints[i] == null) continue;

            if (Ocean != null && Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
        }

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(
                new Vector3(Center.x, WaterLine, Center.z),
                Vector3.one
            );

            Gizmos.DrawRay(
                new Vector3(Center.x, WaterLine, Center.z),
                TargetUp
            );
        }
    }
}
