using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [Tooltip("Pega aquí la URL que quieres abrir")]
    public string url;

    // Este método lo engancharás al OnClick de cada botón
    public void OpenURL()
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
        else
            Debug.LogWarning("OpenLink: falta asignar la URL");
    }
}
