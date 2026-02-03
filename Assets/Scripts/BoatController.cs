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
    [SerializeField] float linearDrag = 1f;
    [SerializeField] float angularDrag = 2f;
    
    
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
    }

    private void FixedUpdate()
    {
        if (boatRb.linearVelocity.magnitude < maxSpeed)
        {
            boatRb.AddForce(transform.forward * (vertical * moveSpeed));
        }
        
        float velocityFactor = Mathf.Clamp01(boatRb.linearVelocity.magnitude / maxSpeed);
        float turnMultiplier = Mathf.Lerp(minTurnSpeed, 1f, velocityFactor);
        boatRb.AddTorque(Vector3.up * (horizontal * turnSpeed * turnMultiplier));
        
    }
}