using System;
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
        if (boatController.currentMovementMode == CurrentMovementMode.Normal)
        {
            Vector3 targetOffset = boatController.lookingRight ? sprintOffset : offset;
            float targetSmoothFollow = boatController.lookingRight ? sprintSmoothFollow : smoothFollow;
            float targetLookAtHeight = boatController.lookingRight ? sprintLookAtHeight : lookAtHeight;
        
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, transitionSpeed * Time.deltaTime);
            currentSmoothFollow = Mathf.Lerp(currentSmoothFollow, targetSmoothFollow, transitionSpeed * Time.deltaTime);
            currentLookAtHeight = Mathf.Lerp(currentLookAtHeight, targetLookAtHeight, transitionSpeed * Time.deltaTime);
        
            Vector3 desiredPosition = target.position + target.TransformDirection(currentOffset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, currentSmoothFollow * Time.deltaTime);
        
            transform.LookAt(target.position + Vector3.up * currentLookAtHeight);
        }
        else if (boatController.currentMovementMode == CurrentMovementMode.Constant)
        {
            MovementModeSpeed mode =  boatController.movementModes[boatController.currentSelectedMovementMode];
            Vector3 targetOffset;
            float targetSmoothFollow;
            float targetLookAtHeight;
                
            if (boatController.lookingRight)
            {
                 targetOffset = mode.modeCamSprintOffset;
                 targetSmoothFollow = mode.modeCamSprintSmoothFollow;
                 targetLookAtHeight = mode.modeCamSprintLookAtHeight;
                
            }
            else if(boatController.lookingLeft)
            {
                 targetOffset = mode.leftShootOffset;
                 targetSmoothFollow = mode.leftShootSmoothFollow;
                 targetLookAtHeight = mode.leftShootLookAtHeight;
            }
            else
            {
                 targetOffset =  mode.modeCamOffset;
                 targetSmoothFollow =  mode.modeCamSmoothFollow;
                 targetLookAtHeight = mode.modeCamLookAtHeight;
            }
            
            
            currentOffset = Vector3.Lerp(currentOffset, targetOffset, transitionSpeed * Time.deltaTime);
            currentSmoothFollow = Mathf.Lerp(currentSmoothFollow, targetSmoothFollow, transitionSpeed * Time.deltaTime);
            currentLookAtHeight = Mathf.Lerp(currentLookAtHeight, targetLookAtHeight, transitionSpeed * Time.deltaTime);
        
            Vector3 desiredPosition = target.position + target.TransformDirection(currentOffset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, currentSmoothFollow * Time.deltaTime);
        
            transform.LookAt(target.position + Vector3.up * currentLookAtHeight);
        }
        
    }
}