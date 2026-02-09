using UnityEngine;

public class RotationPhare : MonoBehaviour
{
    public float vitesseRotation = 30f;

    void Update()
    {
        transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);
    }
}