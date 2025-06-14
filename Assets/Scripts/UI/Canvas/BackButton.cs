using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject desControls;

    public void DesactiveControls()
    {
        // Mostrar el nuevo panel
        desControls.SetActive(false);

    }
}