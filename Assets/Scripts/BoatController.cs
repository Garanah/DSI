using System;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody boatRb;
    
    [Header("Movement Values")]
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float turnSpeed = 1.5f; 
    [SerializeField] float minTurnSpeed = 0.5f; 
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] float backMultiplicator = 0.5f;
    [SerializeField] float linearDrag = 1f;
    [SerializeField] float angularDrag = 2f;
    public bool isSprinting = false;
    
    
    float horizontal;
    float vertical;
    
    void Start()
    {
        if(boatRb == null) boatRb = GetComponent<Rigidbody>();
        
        boatRb.linearDamping = linearDrag;
        boatRb.angularDamping = angularDrag;
    }
    
    private void Update()
    {
        ManageInputs();
    }
    
    private void ManageInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton2);
    }

    private void FixedUpdate()
    {
        if(vertical < 0) vertical *= backMultiplicator;
        float sprintMultiplicator = isSprinting ? 1.75f : 1f;
        if (boatRb.linearVelocity.magnitude < maxSpeed * sprintMultiplicator || PositionFire.shooting!)
        {
            boatRb.AddForce(transform.forward * (vertical * moveSpeed * sprintMultiplicator));
        }
        
        float velocityFactor = Mathf.Clamp01(boatRb.linearVelocity.magnitude / maxSpeed);
        float turnMultiplier = Mathf.Lerp(minTurnSpeed, 1f, velocityFactor);
        boatRb.AddTorque(Vector3.up * (horizontal * turnSpeed * turnMultiplier));
        
    }
}