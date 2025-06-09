using UnityEngine;
using UnityEngine.UI;

public class ActionsButtons : MonoBehaviour
{
    public Button buttonAttack;
    public Button buttonDash;
    public Button buttonJump;

    private void Start()
    {
        buttonAttack.gameObject.SetActive(false);
        buttonDash.gameObject.SetActive(false);
        buttonJump.gameObject.SetActive(false);
    }

    public void UnlockAttack(bool unlock)
    {
        buttonAttack.gameObject.SetActive(unlock);
    }

    public void UnlockDash(bool unlock)
    {
        buttonDash.gameObject.SetActive(unlock);
    }

    public void UnlockJump(bool unlock)
    {
        buttonJump.gameObject.SetActive(unlock);        
    }
}
