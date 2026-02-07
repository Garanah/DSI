using System;
using UnityEngine;

public class OceanFollow : MonoBehaviour
{
    public Transform target;
    public float yLevel = 0f;
    public float followSmooth = 0.15f;
    public float smooth = 0.15f;
    public float snapSize = 5f;

    private Vector3 velocity;
    private Vector3 snappedPos;

    void LateUpdate()
    {
        if (!target) return;

        // snappedPos.x = Mathf.Floor(target.position.x / snapSize) * snapSize;
        // snappedPos.z = Mathf.Floor(target.position.z / snapSize) * snapSize;
        // snappedPos.y = yLevel;
        
        Vector3 desired = new Vector3(target.position.x, yLevel, target.position.z);
        
        //transform.position = Vector3.SmoothDamp(transform.position, snappedPos, ref velocity, followSmooth);
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, followSmooth);
    }

    private void Start()
    {
        transform.position = new  Vector3(target.position.x, yLevel, target.position.z);
    }
}