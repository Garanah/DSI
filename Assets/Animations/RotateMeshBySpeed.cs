using UnityEngine;

[System.Serializable]
public class RotationSpeedByMode
{
    public string modeName;
    public float rotationSpeed;
}

public class RotateMeshBySpeed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoatController boatController;

    [Header("Rotation Settings")]
    public RotationSpeedByMode[] rotationByMode;

    [SerializeField] float acceleration = 5f;

    float currentRotationSpeed;
    float targetRotationSpeed;

    void Start()
    {
        UpdateTargetSpeed();
    }

    void OnEnable()
    {
        if (boatController != null)
            boatController.OnMovementModeChanged += UpdateTargetSpeed;
    }

    void OnDisable()
    {
        if (boatController != null)
            boatController.OnMovementModeChanged -= UpdateTargetSpeed;
    }

    void Update()
    {
        currentRotationSpeed = Mathf.Lerp(
            currentRotationSpeed,
            targetRotationSpeed,
            Time.deltaTime * acceleration
        );
        
        transform.Rotate(Vector3.right, currentRotationSpeed * Time.deltaTime, Space.Self);
    }

    void UpdateTargetSpeed()
    {
        int index = boatController.currentSelectedMovementMode;

        if (index < 0 || index >= rotationByMode.Length)
            return;

        targetRotationSpeed = rotationByMode[index].rotationSpeed;
    }
}