using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FutureEventCountdown : MonoBehaviour
{
    [SerializeField] private GameObject countdownRoot;
    [SerializeField] private Text countdownText;
    [SerializeField] private Button eventOpenButton;

    private DateTime eventDate;
    private bool hasValidDate;
    private bool deleteVisible;
    private bool canOpenWhenReady;
    private Coroutine refreshRoutine;

    public void Initialize(string date, bool canOpen)
    {
        canOpenWhenReady = canOpen;
        hasValidDate = EventDateParser.TryParse(date, out eventDate);

        if (refreshRoutine != null)
            StopCoroutine(refreshRoutine);

        RefreshState();

        if (hasValidDate)
            refreshRoutine = StartCoroutine(RefreshAtMidnight());
    }

    public void SetDeleteVisible(bool visible)
    {
        deleteVisible = visible;
        RefreshState();
    }

    private void RefreshState()
    {
        if (!hasValidDate)
        {
            countdownRoot.SetActive(false);
            eventOpenButton.interactable = canOpenWhenReady;
            return;
        }

        int daysLeft = (eventDate.Date - DateTime.Today).Days;
        bool isFuture = daysLeft > 0;

        countdownRoot.SetActive(isFuture && !deleteVisible);
        eventOpenButton.interactable = canOpenWhenReady;

        if (isFuture)
            countdownText.text = daysLeft.ToString();
    }

    private IEnumerator RefreshAtMidnight()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(60f);
            RefreshState();
        }
    }

    public static bool IsFutureEvent(string value)
    {
        return EventDateParser.TryParse(value, out DateTime date) &&
               date.Date > DateTime.Today;
    }

}
