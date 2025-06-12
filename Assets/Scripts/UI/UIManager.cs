using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject keyIcon;
    public TMP_Text messageText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowKeyIcon()
    {
        keyIcon.SetActive(true);
    }

    public void ShowMessage(string msg, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine(msg, duration));
    }

    IEnumerator ShowMessageCoroutine(string msg, float duration)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
    }
}
