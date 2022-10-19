using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] float _duration;

    private void OnEnable()
    {
        StartCoroutine(FadeIn(_duration));
    }

    public void EnableAndFade()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn(_duration));
    }

    public void DisableAndFade()
    {
        StartCoroutine(FadeOut(_duration));
        gameObject.SetActive(false);
    }

    IEnumerator FadeIn(float duration)
    {
        _canvasGroup.alpha = 0;

        float elapsedTime = 0;
        float progress = 0;
        while (progress < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }

        _canvasGroup.alpha = 1;
    }

    IEnumerator FadeOut(float duration)
    {
        _canvasGroup.alpha = 1;

        float elapsedTime = 0;
        float progress = 0;
        while (progress < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }

        _canvasGroup.alpha = 0;
    }
}
