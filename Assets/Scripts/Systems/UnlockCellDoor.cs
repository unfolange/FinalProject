using UnityEngine;

public class UnlockCellDoor : MonoBehaviour
{
    [Tooltip("Referencia al controlador de la puerta de la celda")]
    public DoorController cellDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cellDoor != null)
        {
            cellDoor.UnlockDoor();
            Destroy(gameObject);  // Destruye orbe
            // UIManager.Instance.ShowMessage("¡Puerta desbloqueada!", 2f);
        }
    }
}
