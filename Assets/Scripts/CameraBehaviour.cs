using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] float smoothFollow = 5f;
    [SerializeField] float lookAtHeight = 2f;

    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothFollow * Time.deltaTime);
        
        transform.LookAt(target.position + Vector3.up * lookAtHeight);
    }
}