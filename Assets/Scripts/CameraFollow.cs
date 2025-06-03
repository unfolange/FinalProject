using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Velocidad de seguimiento de la c�mara
    [SerializeField] private float followSpeed = 0.1f;

    //Distancia de la c�mara al player
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position + offset, followSpeed);
    }
}
