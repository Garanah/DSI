using UnityEngine;

public class FollowCameraOcean : MonoBehaviour
{
    public Transform target;   // camera or boat 
    public float yOffset = 0f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = target.position;
        pos.y = yOffset;
        transform.position = pos;
    }
}