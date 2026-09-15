using System.Collections;
using UnityEngine;

public class CopyNotification : MonoBehaviour
{
    [SerializeField] private CanvasGroup notificationCanvasGroup;

    [SerializeField] private float fadeInDuration = 0.25f;
    [SerializeField] private float displayDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private Coroutine notificationRoutine;

    private void Start()
    {
        // Start completely invisible
        notificationCanvasGroup.alpha = 0f;
    }

    public void ShowCopiedMessage()
    {
        // If already showing, restart the animation
        if (notificationRoutine != null)
            StopCoroutine(notificationRoutine);

        notificationRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        // FADE IN
        yield return Fade(0f, 1f, fadeInDuration);

        // STAY VISIBLE
        yield return new WaitForSeconds(displayDuration);

        // FADE OUT
        yield return Fade(1f, 0f, fadeOutDuration);

        notificationRoutine = null;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            notificationCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            yield return null;
        }

        notificationCanvasGroup.alpha = endAlpha;
    }
}