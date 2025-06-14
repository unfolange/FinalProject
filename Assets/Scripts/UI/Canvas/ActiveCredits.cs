using UnityEngine;

public class ActiveCredits : MonoBehaviour
{
    public GameObject credits;

    public void ActCredits()
    {
        // Mostrar el nuevo panel
        credits.SetActive(true);
    }
}
