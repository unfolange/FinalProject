using UnityEngine;

public class ControlsButton : MonoBehaviour
{
    public GameObject controlsImage;

    public void ShowControls()
    {
        // Mostrar el nuevo panel
        controlsImage.SetActive(true);

    }
}