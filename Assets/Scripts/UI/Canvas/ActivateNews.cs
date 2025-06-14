using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateNews : MonoBehaviour
{
    public GameObject news;          // Panel que se va a activar
    public Image fadeImage;          // Imagen negra para el fade
    public float fadeDuration = 0.5f;

    public void ShowNews()
    {
        StartCoroutine(FadeAndShowPanel());
    }

    IEnumerator FadeAndShowPanel()
    {
        // Fade In (a negro)
        yield return StartCoroutine(Fade(0, 1));

        // Mostrar el panel mientras está negro
        news.SetActive(true);

        // Fade Out (desvanece el negro)
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
