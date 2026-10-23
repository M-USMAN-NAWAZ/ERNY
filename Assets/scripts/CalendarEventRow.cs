using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class CalendarEventRow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject divider;
    [SerializeField] private Button deleteButton;
    [SerializeField] private RectTransform title;
    private ScrollRect scroll;
    private bool horizontal;
    private bool dragging;
    private RectTransform row;
    private RectTransform type;
    private float visualOffset;
    private float dragStartOffset;
    private Vector2 titleOffset;

    public void Initialize(bool manual, bool canDelete, Action delete)
    {
        divider.SetActive(true);
        enabled = manual;
        GetComponent<Image>().raycastTarget = manual;
        scroll = GetComponentInParent<ScrollRect>();
        row = (RectTransform)transform;
        type = transform.Find("Type").GetComponent<RectTransform>();
        titleOffset = title.offsetMax;
        deleteButton.interactable = canDelete;
        deleteButton.onClick.AddListener(() => delete());
        ShowDelete(false);
    }

    private void ShowDelete(bool visible)
    {
        float offset = visualOffset;
        SetOffset(0f);
        deleteButton.gameObject.SetActive(visible);
        float width = ((RectTransform)deleteButton.transform).rect.width;
        title.offsetMax = titleOffset - new Vector2(visible ? width + 8f : 0f, 0f);
        SetOffset(offset);
    }

    private void SetOffset(float offset)
    {
        Vector2 movement = new Vector2(offset - visualOffset, 0f);
        title.anchoredPosition += movement;
        type.anchoredPosition += movement;
        visualOffset = offset;
    }

    private void Update()
    {
        if (!dragging && visualOffset != 0f)
        {
            float offset = Mathf.Lerp(visualOffset, 0f, 1f - Mathf.Exp(-20f * Time.unscaledDeltaTime));
            SetOffset(Mathf.Abs(offset) < 0.05f ? 0f : offset);
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        horizontal = Mathf.Abs(data.delta.x) > Mathf.Abs(data.delta.y);
        dragging = horizontal;
        dragStartOffset = visualOffset;
        if (!horizontal && scroll != null)
            scroll.OnBeginDrag(data);
    }

    public void OnDrag(PointerEventData data)
    {
        if (horizontal)
        {
            Vector2 start;
            Vector2 current;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(row, data.pressPosition, data.pressEventCamera, out start)
                && RectTransformUtility.ScreenPointToLocalPointInRectangle(row, data.position, data.pressEventCamera, out current))
                SetOffset(Mathf.Clamp(dragStartOffset + (current.x - start.x) * 0.5f, -row.rect.width * 0.25f, row.rect.width * 0.25f));
        }
        else if (scroll != null)
            scroll.OnDrag(data);
    }

    public void OnEndDrag(PointerEventData data)
    {
        dragging = false;
        if (!horizontal)
        {
            if (scroll != null)
                scroll.OnEndDrag(data);
            return;
        }
        float distance = data.position.x - data.pressPosition.x;
        if (Mathf.Abs(distance) >= 30f)
            ShowDelete(!deleteButton.gameObject.activeSelf);
    }
}
