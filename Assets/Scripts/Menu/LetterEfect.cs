using UnityEngine;

public class LetterEfect : MonoBehaviour
{
    public float scaleAmplitude = 0.05f; // cuánto escala
    public float scaleFrequency = 2f;    // velocidad de escala

    public float rotationAmplitude = 5f; // grados de rotación
    public float rotationFrequency = 2f; // velocidad de rotación

    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * scaleFrequency) * scaleAmplitude;
        transform.localScale = initialScale * scale;

        float angle = Mathf.Sin(Time.time * rotationFrequency) * rotationAmplitude;
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, angle);
    }
}
