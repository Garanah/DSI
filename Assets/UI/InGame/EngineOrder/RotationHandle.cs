using UnityEngine;

[System.Serializable]
public class EngineOrderRotation
{
    public string orderName;
    public float zRotation;
}

public class RotationHandle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform handle;
    [SerializeField] BoatController boatController;

    [Header("Engine Order Rotations")]
    [Tooltip("Index = currentSelectedMovementMode")]
    public EngineOrderRotation[] rotations;

    [Header("Animation")]
    [SerializeField] float rotationSpeed = 6f;

    float currentAngle;
    float targetAngle;

    void Start()
    {
        currentAngle = handle.localEulerAngles.z;
        UpdateTargetRotation();
        handle.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void Update()
    {
        currentAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            Time.deltaTime * rotationSpeed
        );

        handle.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void OnEnable()
    {
        if (boatController != null)
            boatController.OnMovementModeChanged += UpdateTargetRotation;
    }

    void OnDisable()
    {
        if (boatController != null)
            boatController.OnMovementModeChanged -= UpdateTargetRotation;
    }

    public void UpdateTargetRotation()
    {
        int index = boatController.currentSelectedMovementMode;

        if (index < 0 || index >= rotations.Length)
            return;

        targetAngle = rotations[index].zRotation;
    }
}