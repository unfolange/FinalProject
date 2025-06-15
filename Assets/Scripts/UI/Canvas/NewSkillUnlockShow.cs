using UnityEngine;
using System.Collections;

public class NewSkillUnlockShow : MonoBehaviour
{
    [SerializeField] private GameObject skillUnlockUI;


    public void Show()
    {
        skillUnlockUI.SetActive(true);

#if UNITY_ANDROID
        skillUnlockUI.transform.GetChild(1).gameObject.SetActive(false);
#endif


        StartCoroutine(HideSkillUnlockUI());
    }

    private IEnumerator HideSkillUnlockUI()
    {
        yield return new WaitForSecondsRealtime(3f);
        skillUnlockUI.SetActive(false);
    }
}
