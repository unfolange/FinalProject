using UnityEngine;
using System.Collections;

public class NewSkillUnlockShow : MonoBehaviour
{
    [SerializeField] private GameObject skillUnlockUI;


    public void Show()
    {
        skillUnlockUI.SetActive(true);
        StartCoroutine(HideSkillUnlockUI());
    }

    private IEnumerator HideSkillUnlockUI()
    {
        yield return new WaitForSecondsRealtime(3f);
        skillUnlockUI.SetActive(false);
    }
}
