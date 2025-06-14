using UnityEngine;

public class DesactiveCredits : MonoBehaviour
{
    public GameObject desCredits;
    public GameObject desBG;

    public void DesCredits()
    {
        // Mostrar el nuevo panel
        desCredits.SetActive(false);

    }

    public void DesBG()
    {
        desBG.SetActive(false);
    }
}