using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Button))] // Asegura que hay un componente Button
public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerController playerController;
    private bool buttonPressed;

    private void Awake()
    {
        // Busca automáticamente el PlayerController si no está asignado
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        playerController?.OnJumpButtonDown(); // Llamada inmediata
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        playerController?.OnJumpButtonUp();
    }
}