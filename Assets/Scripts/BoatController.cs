using System;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public CurrentMovementMode currentMovementMode;
    public List<MovementModeSpeed>  movementModes;
    public int currentSelectedMovementMode = 0;

    [Header("Components")]
    [SerializeField] Rigidbody boatRb;
    private bool isStopped = false;

    [Header("Movement Values")] 
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float turnSpeed = 1.5f;
    [SerializeField] float minTurnSpeed = 0.5f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] private float backMultiplicator = 0.5f;
        
    [Header("Drag")]
    [SerializeField] float linearDrag = 1f;
    [SerializeField] float angularDrag = 2f;
    public bool isSprinting = false;

    [Header("UI Animation")]
    public System.Action OnMovementModeChanged;
    
    float horizontal;
    float vertical;

    void Start()
    {
        if (boatRb == null) boatRb = GetComponent<Rigidbody>();

        boatRb.linearDamping = linearDrag;
        boatRb.angularDamping = angularDrag;
    }

    private void Update()
    {
        ManageInputs();

        if (movementModes.Count == 0 && currentMovementMode == CurrentMovementMode.Constant) currentMovementMode = CurrentMovementMode.Normal; 
    }

    private void ManageInputs()
    {
        if (isStopped) return;
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton5);

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (currentSelectedMovementMode + 1 < movementModes.Count)
            {
                currentSelectedMovementMode++;
                OnMovementModeChanged?.Invoke();
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            
            if (currentSelectedMovementMode > 0)
            {
                currentSelectedMovementMode--;
                OnMovementModeChanged?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStopped) return;
        
        if (currentMovementMode == CurrentMovementMode.Normal)
        {
            if (vertical < 0) vertical *= backMultiplicator;
            float sprintMultiplicator = isSprinting ? 1.75f : 1f;
            if (boatRb.linearVelocity.magnitude < maxSpeed)
            {
                boatRb.AddForce(transform.forward * (vertical * moveSpeed));
            }

            float velocityFactor = Mathf.Clamp01(boatRb.linearVelocity.magnitude / maxSpeed);
            float turnMultiplier = Mathf.Lerp(minTurnSpeed, 1f, velocityFactor);
            boatRb.AddTorque(Vector3.up * (horizontal * turnSpeed * turnMultiplier));
        }
        else if(currentMovementMode == CurrentMovementMode.Constant)
        {
            MovementModeSpeed mode = movementModes[currentSelectedMovementMode];
            float sprintMultiplier = isSprinting ? 1.75f : 1f;
            float targetSpeed = mode.modeMoveSpeed;
        
            // Apply constant forward force
            if (boatRb.linearVelocity.magnitude < targetSpeed)
            {
                boatRb.AddForce(transform.forward * mode.modeMoveSpeed );
            }

            float velocityFactor = Mathf.Clamp01(boatRb.linearVelocity.magnitude / targetSpeed);
            float turnMultiplier = Mathf.Lerp(mode.modeMinTurnSpeed, 1f, velocityFactor);
            boatRb.AddTorque(Vector3.up * (horizontal * mode.modeTurnSpeed * turnMultiplier)); //je l'ai commenté car je ne pouvais plus play
        }
        
    }
    // MOORING
    public void StopBoat()
    {
        if (boatRb == null) return;

        isStopped = true;
        
        boatRb.linearVelocity = Vector3.zero;
        boatRb.angularVelocity = Vector3.zero;
        //boatRb.isKinematic = true;
        currentSelectedMovementMode = 0;
        OnMovementModeChanged?.Invoke();
    }

    public void ResumeBoat()
    {
        if (boatRb == null) return;

        isStopped = false;
        //boatRb.isKinematic = false;
    }
}

[Serializable]
public class MovementModeSpeed
{
    [Header("Movement Data")]
    public string modeName;
    public float modeMoveSpeed = 15f;
    public float modeTurnSpeed = 1.5f;
    public float modeMinTurnSpeed = 0.5f;
    
    [Header("Camera Data Normal")]
    public Vector3 modeCamOffset = new Vector3(0, 5, -10);
    public float modeCamSmoothFollow = 5f;
    public float modeCamLookAtHeight = 2f;
    
    [Header("Camera Sprint Mode Settings")]
    public Vector3 modeCamSprintOffset = new Vector3(0, 6, -12);
    public float modeCamSprintSmoothFollow = 8f;
    public float modeCamSprintLookAtHeight = 2.5f;
    
}

[Serializable]
public enum CurrentMovementMode
{
    Constant,
    Normal
};