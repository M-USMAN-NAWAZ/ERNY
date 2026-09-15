using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoldCountdownVisual : MonoBehaviour
{
    public Image buttonFill;     // Main shrinking image
    public Image countdownFill;  // Expanding countdown image
    public float duration = 10f;

    private Coroutine countdownRoutine;

    void Start()
    {
        ResetVisual();
    }

    public void StartCountdown()
    {
        StopCountdown(); // Ensure no duplicates
        countdownRoutine = StartCoroutine(DoCountdown());
    }

    public void StopCountdown()
    {
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = null;
        ResetVisual();
    }

    private IEnumerator DoCountdown()
    {
        float start = Time.time;

        while (Time.time - start < duration)
        {
            float ratio = (Time.time - start) / duration;

            if (buttonFill)
                buttonFill.fillAmount = 1f - ratio;

            if (countdownFill)
                countdownFill.fillAmount = ratio;

            yield return null;
        }

        // Reached end:
        ResetVisual();
        countdownRoutine = null;
    }

    private void ResetVisual()
    {
        if (buttonFill) buttonFill.fillAmount = 1f;
        if (countdownFill) countdownFill.fillAmount = 0f;
    }
}
