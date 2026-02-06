using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    
    [Header("Normal Mode Settings")]
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] float smoothFollow = 5f;
    [SerializeField] float lookAtHeight = 2f;
    
    [Header("Sprint Mode Settings")]
    [SerializeField] Vector3 sprintOffset = new Vector3(0, 6, -12);
    [SerializeField] float sprintSmoothFollow = 8f;
    [SerializeField] float sprintLookAtHeight = 2.5f;
    
    [Header("Transition")]
    [SerializeField] float transitionSpeed = 3f;
    
    [SerializeField] BoatController boatController;
    
    private Vector3 currentOffset;
    private float currentSmoothFollow;
    private float currentLookAtHeight;

    void Start()
    {
        currentOffset = offset;
        currentSmoothFollow = smoothFollow;
        currentLookAtHeight = lookAtHeight;
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetOffset = boatController.isSprinting ? sprintOffset : offset;
        float targetSmoothFollow = boatController.isSprinting ? sprintSmoothFollow : smoothFollow;
        float targetLookAtHeight = boatController.isSprinting ? sprintLookAtHeight : lookAtHeight;
        
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, transitionSpeed * Time.deltaTime);
        currentSmoothFollow = Mathf.Lerp(currentSmoothFollow, targetSmoothFollow, transitionSpeed * Time.deltaTime);
        currentLookAtHeight = Mathf.Lerp(currentLookAtHeight, targetLookAtHeight, transitionSpeed * Time.deltaTime);
        
        Vector3 desiredPosition = target.position + target.TransformDirection(currentOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, currentSmoothFollow * Time.deltaTime);
        
        transform.LookAt(target.position + Vector3.up * currentLookAtHeight);
    }
}