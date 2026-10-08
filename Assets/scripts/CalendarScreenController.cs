using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public sealed class CalendarScreenController : MonoBehaviour
{
    public const int ScreenIndex = 5;

    private static readonly Color Navy = Hex("#29375F");
    private static readonly Color Yellow = Hex("#F7C85A");
    private static readonly Color Green = Hex("#97B56F");
    private static readonly Color Red = Hex("#C15462");

    public static CalendarScreenController Instance { get; private set; }

    private readonly List<CalendarItem> items = new List<CalendarItem>();
    private readonly List<DayCell> dayCells = new List<DayCell>();

    private eventgetter eventScreen;
    private GameObject root;
    private GameObject calendarSelectedIcon;
    [SerializeField] private Text monthText;
    [SerializeField] private Text statusText;
    [SerializeField] private RectTransform listContent;
    [SerializeField] private GameObject listRowTemplate;
    [SerializeField] private Button previousMonthButton;
    [SerializeField] private Button nextMonthButton;
    [SerializeField] private Button addEventButton;
    [SerializeField] private Button[] dateButtons;
    private Font font;
    private Sprite circleSprite;
    private DateTime displayedMonth;
    private DateTime? pendingManualDate;
    private int requestVersion;

    public static void EnsureCreated(eventgetter owner)
    {
        if (owner == null || owner.canv == null)
        {
            return;
        }

        if (Instance != null)
        {
            Instance.eventScreen = owner;
            return;
        }

        CalendarScreenController savedController = owner.canv
            .GetComponentInChildren<CalendarScreenController>(true);
        if (savedController != null)
        {
            Instance = savedController;
            Instance.Initialize(owner);
            return;
        }

        GameObject controllerObject = new GameObject(
            "Calendar Screen",
            typeof(RectTransform),
            typeof(Image),
            typeof(CalendarScreenController));
        controllerObject.transform.SetParent(owner.canv.transform, false);

        Instance = controllerObject.GetComponent<CalendarScreenController>();
        Instance.Initialize(owner);
    }

    public static bool TrySubmitPendingManualEvent(eventgetter owner)
    {
        if (Instance == null || !Instance.pendingManualDate.HasValue)
        {
            return false;
        }

        Instance.SubmitPendingManualEvent(owner);
        return true;
    }

    public static void CancelPendingCreation()
    {
        if (Instance != null)
        {
            Instance.pendingManualDate = null;
        }
    }

    private void Initialize(eventgetter owner)
    {
        eventScreen = owner;
        root = gameObject;
        gameObject.layer = 5;
        displayedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        font = owner.name != null && owner.name.font != null
            ? owner.name.font
            : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        PlaceBelowEventOverlays();

        circleSprite = CreateCircleSprite();
        if (monthText == null && transform.Find("Month") == null)
        {
            Stretch(GetComponent<RectTransform>());
            GetComponent<Image>().color = Color.white;
            BuildInterface();
        }
        else
        {
            BindSavedInterface();
        }
        WireCalendarNavigationButton();
        root.SetActive(false);

        if (eventgetter.currentscreen == ScreenIndex)
        {
            Show();
        }
    }

    private void WireCalendarNavigationButton()
    {
        if (eventScreen.bottom == null)
        {
            return;
        }

        Button calendarButton = eventScreen.bottom
            .GetComponentsInChildren<Button>(true)
            .FirstOrDefault(button => button.name == "profile (2)");

        if (calendarButton == null)
        {
            Debug.LogError("Calendar navigation button 'profile (2)' was not found.");
            return;
        }

        Transform selectedIcon = calendarButton.transform.Find("Image");
        calendarSelectedIcon = selectedIcon != null ? selectedIcon.gameObject : null;

        // This icon was duplicated from Profile and still has Profile callbacks.
        calendarButton.onClick = new Button.ButtonClickedEvent();
        calendarButton.onClick.AddListener(
            () => eventScreen.screencurenter(ScreenIndex));
    }

    private void BindSavedInterface()
    {
        if (monthText == null)
            monthText = transform.Find("Month").GetComponent<Text>();
        if (statusText == null)
            statusText = transform.Find("Calendar Status").GetComponent<Text>();
        if (listContent == null)
            listContent = transform.Find("Calendar Event List/Viewport/Content").GetComponent<RectTransform>();
        if (listRowTemplate == null)
        {
            Transform template = transform.Find("Event Row Template");
            listRowTemplate = template != null ? template.gameObject : null;
        }
        if (listRowTemplate != null)
        {
            listRowTemplate.SetActive(false);
        }

        if (previousMonthButton == null)
            previousMonthButton = transform.Find("Previous Month").GetComponent<Button>();
        if (nextMonthButton == null)
            nextMonthButton = transform.Find("Next Month").GetComponent<Button>();
        if (addEventButton == null)
            addEventButton = transform.Find("Add Calendar Event").GetComponent<Button>();
        previousMonthButton.onClick.AddListener(() => ChangeMonth(-1));
        nextMonthButton.onClick.AddListener(() => ChangeMonth(1));
        addEventButton.onClick.AddListener(() => OpenCreateForDate(DefaultCreationDate()));

        dayCells.Clear();
        for (int index = 0; index < 42; index++)
        {
            Button button = dateButtons != null && dateButtons.Length == 42
                ? dateButtons[index]
                : transform.Find("Day " + index).GetComponent<Button>();
            Image background = button.GetComponent<Image>();
            if (background.sprite == null)
            {
                background.sprite = circleSprite;
            }
            dayCells.Add(new DayCell(button, button.GetComponentInChildren<Text>(), background));
        }
    }

    public void Show()
    {
        if (root == null)
        {
            return;
        }

        root.SetActive(true);
        PlaceBelowEventOverlays();

        if (calendarSelectedIcon != null)
        {
            calendarSelectedIcon.SetActive(true);
        }

        RenderMonthShell();
        LoadDisplayedMonth();
    }

    private void PlaceBelowEventOverlays()
    {
        Transform parent = root.transform.parent;
        int index = parent.childCount - 1;

        if (eventScreen.help != null && eventScreen.help.transform.parent == parent)
        {
            index = eventScreen.help.transform.GetSiblingIndex() + 1;
        }

        root.transform.SetSiblingIndex(Mathf.Clamp(index, 0, parent.childCount - 1));
    }

    public void Hide()
    {
        requestVersion++;
        if (root != null)
        {
            root.SetActive(false);
        }

        if (calendarSelectedIcon != null)
        {
            calendarSelectedIcon.SetActive(false);
        }
    }

    private void BuildInterface()
    {
        Image topBand = CreateImage("Top Band", transform, Yellow);
        SetAnchors(topBand.rectTransform, 0f, 0.92f, 1f, 1f);

        monthText = CreateText(
            "Month",
            transform,
            string.Empty,
            38,
            Red,
            TextAnchor.MiddleLeft,
            FontStyle.Bold);
        SetAnchors(monthText.rectTransform, 0.08f, 0.79f, 0.57f, 0.91f);

        Button previous = CreateCircleButton("Previous Month", transform, "<", Navy, 38);
        SetAnchors(previous.GetComponent<RectTransform>(), 0.63f, 0.815f, 0.75f, 0.885f);
        previous.onClick.AddListener(() => ChangeMonth(-1));

        Button next = CreateCircleButton("Next Month", transform, ">", Navy, 38);
        SetAnchors(next.GetComponent<RectTransform>(), 0.80f, 0.815f, 0.92f, 0.885f);
        next.onClick.AddListener(() => ChangeMonth(1));

        Image topDivider = CreateImage("Top Divider", transform, Navy);
        SetAnchors(topDivider.rectTransform, 0.08f, 0.775f, 0.92f, 0.779f);

        string[] weekdayNames = { "S", "M", "T", "W", "T", "F", "S" };
        for (int column = 0; column < weekdayNames.Length; column++)
        {
            float left = 0.06f + column * 0.88f / 7f;
            Text weekday = CreateText(
                "Weekday " + weekdayNames[column],
                transform,
                weekdayNames[column],
                27,
                Navy,
                TextAnchor.MiddleCenter,
                FontStyle.Bold);
            SetAnchors(weekday.rectTransform, left, 0.715f, left + 0.88f / 7f, 0.765f);
        }

        for (int index = 0; index < 42; index++)
        {
            int row = index / 7;
            int column = index % 7;
            float left = 0.06f + column * 0.88f / 7f;
            float top = 0.71f - row * 0.285f / 6f;
            float bottom = top - 0.285f / 6f;

            Button button = CreateCircleButton(
                "Day " + index,
                transform,
                string.Empty,
                new Color(1f, 1f, 1f, 0f),
                25);
            RectTransform rect = button.GetComponent<RectTransform>();
            SetAnchors(rect, left + 0.021f, bottom + 0.004f, left + 0.88f / 7f - 0.021f, top - 0.004f);

            Text label = button.GetComponentInChildren<Text>();
            label.color = Navy;
            dayCells.Add(new DayCell(button, label, button.GetComponent<Image>()));
        }

        Image listDivider = CreateImage("List Divider", transform, Navy);
        SetAnchors(listDivider.rectTransform, 0.08f, 0.402f, 0.92f, 0.406f);

        ScrollRect list = CreateScrollView();
        SetAnchors(list.GetComponent<RectTransform>(), 0.07f, 0.18f, 0.93f, 0.392f);

        statusText = CreateText(
            "Calendar Status",
            transform,
            string.Empty,
            23,
            Navy,
            TextAnchor.MiddleCenter,
            FontStyle.Normal);
        SetAnchors(statusText.rectTransform, 0.1f, 0.25f, 0.9f, 0.33f);
        statusText.raycastTarget = false;

        Button add = CreateCircleButton("Add Calendar Event", transform, "+", Navy, 46);
        SetAnchors(add.GetComponent<RectTransform>(), 0.43f, 0.105f, 0.57f, 0.17f);
        add.onClick.AddListener(() => OpenCreateForDate(DefaultCreationDate()));
    }

    private ScrollRect CreateScrollView()
    {
        GameObject scrollObject = new GameObject(
            "Calendar Event List",
            typeof(RectTransform),
            typeof(ScrollRect));
        scrollObject.layer = 5;
        scrollObject.transform.SetParent(transform, false);

        GameObject viewportObject = new GameObject(
            "Viewport",
            typeof(RectTransform),
            typeof(RectMask2D));
        viewportObject.layer = 5;
        viewportObject.transform.SetParent(scrollObject.transform, false);
        RectTransform viewport = viewportObject.GetComponent<RectTransform>();
        Stretch(viewport);

        GameObject contentObject = new GameObject(
            "Content",
            typeof(RectTransform),
            typeof(VerticalLayoutGroup),
            typeof(ContentSizeFitter));
        contentObject.layer = 5;
        contentObject.transform.SetParent(viewportObject.transform, false);
        listContent = contentObject.GetComponent<RectTransform>();
        listContent.anchorMin = new Vector2(0f, 1f);
        listContent.anchorMax = new Vector2(1f, 1f);
        listContent.pivot = new Vector2(0.5f, 1f);
        listContent.anchoredPosition = Vector2.zero;
        listContent.sizeDelta = Vector2.zero;

        VerticalLayoutGroup layout = contentObject.GetComponent<VerticalLayoutGroup>();
        layout.spacing = 6f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter fitter = contentObject.GetComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect scroll = scrollObject.GetComponent<ScrollRect>();
        scroll.viewport = viewport;
        scroll.content = listContent;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 18f;
        return scroll;
    }

    private void ChangeMonth(int offset)
    {
        displayedMonth = displayedMonth.AddMonths(offset);
        RenderMonthShell();
        LoadDisplayedMonth();
    }

    private void RenderMonthShell()
    {
        monthText.text = displayedMonth.ToString("MMMM", CultureInfo.InvariantCulture).ToUpperInvariant()
            + "\n"
            + displayedMonth.Year;

        int daysInMonth = DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month);
        int firstColumn = (int)displayedMonth.DayOfWeek;

        for (int index = 0; index < dayCells.Count; index++)
        {
            DayCell cell = dayCells[index];
            cell.Button.onClick.RemoveAllListeners();
            cell.Background.color = new Color(1f, 1f, 1f, 0f);

            int day = index - firstColumn + 1;
            bool validDay = day >= 1 && day <= daysInMonth;
            cell.Button.interactable = validDay;
            cell.Label.text = validDay ? day.ToString(CultureInfo.InvariantCulture) : string.Empty;

            if (validDay)
            {
                DateTime date = new DateTime(displayedMonth.Year, displayedMonth.Month, day);
                cell.Button.onClick.AddListener(() => OpenCreateForDate(date));
            }
        }

        ClearList();
        statusText.text = "Loading events...";
        statusText.gameObject.SetActive(true);
    }

    private void LoadDisplayedMonth()
    {
        int version = ++requestVersion;
        StartCoroutine(LoadMonth(version));
    }

    private IEnumerator LoadMonth(int version)
    {
        DateTime start = displayedMonth;
        DateTime end = displayedMonth.AddMonths(1).AddDays(-1);
        string url = ApiBaseUrl()
            + "/v1/calendar?startDate=" + start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            + "&endDate=" + end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (version != requestVersion || root == null || !root.activeInHierarchy)
            {
                yield break;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                statusText.text = "Unable to load calendar events.";
                statusText.gameObject.SetActive(true);
                Debug.LogError("Calendar load failed (" + request.responseCode + "): " + request.downloadHandler.text);
                yield break;
            }

            if (!TryParseItems(request.downloadHandler.text, out string parseError))
            {
                statusText.text = "Unable to read calendar events.";
                statusText.gameObject.SetActive(true);
                Debug.LogError(parseError);
                yield break;
            }
        }

        RenderItems();
    }

    private bool TryParseItems(string json, out string error)
    {
        items.Clear();

        try
        {
            JArray array = JToken.Parse(json)["response"] as JArray;
            if (array == null)
            {
                error = "Calendar response did not contain an event array.";
                return false;
            }

            foreach (JObject entry in array.OfType<JObject>())
            {
                bool isManual = string.Equals(
                    (string)entry["itemType"],
                    "manual",
                    StringComparison.OrdinalIgnoreCase);
                string dateValue = isManual
                    ? (string)entry["date"]
                    : (string)entry["eventDate"];

                if (!TryParseCalendarDate(dateValue, out DateTime date))
                {
                    continue;
                }

                items.Add(new CalendarItem
                {
                    IsManual = isManual,
                    Title = isManual
                        ? (string)entry["title"]
                        : (string)entry["eventName"],
                    Date = date.Date
                });
            }

            error = null;
            return true;
        }
        catch (Exception exception)
        {
            error = "Calendar response parse failed: " + exception.Message;
            return false;
        }
    }

    private void RenderItems()
    {
        foreach (DayCell cell in dayCells)
        {
            cell.Background.color = new Color(1f, 1f, 1f, 0f);
        }

        int firstColumn = (int)displayedMonth.DayOfWeek;
        foreach (IGrouping<DateTime, CalendarItem> group in items.GroupBy(item => item.Date))
        {
            if (group.Key.Year != displayedMonth.Year || group.Key.Month != displayedMonth.Month)
            {
                continue;
            }

            int index = firstColumn + group.Key.Day - 1;
            if (index < 0 || index >= dayCells.Count)
            {
                continue;
            }

            dayCells[index].Background.color = group.Any(item => item.IsManual) ? Red : Green;
        }

        ClearList();
        bool hasManualEvents = items.Any(item => item.IsManual);
        List<CalendarItem> visibleItems = items
            .Where(item => hasManualEvents ? item.IsManual : !item.IsManual)
            .OrderBy(item => item.Date)
            .ThenBy(item => item.Title)
            .ToList();

        statusText.gameObject.SetActive(visibleItems.Count == 0);
        statusText.text = visibleItems.Count == 0 ? "No events this month." : string.Empty;

        foreach (CalendarItem item in visibleItems)
        {
            CreateListRow(item);
        }
    }

    private void CreateListRow(CalendarItem item)
    {
        if (listRowTemplate != null)
        {
            GameObject savedRow = Instantiate(listRowTemplate, listContent, false);
            savedRow.name = "Calendar Item " + item.Title;
            savedRow.transform.Find("Type").GetComponent<Image>().color = item.IsManual ? Red : Green;
            savedRow.transform.Find("Title").GetComponent<Text>().text =
                item.Date.ToString("MMM d", CultureInfo.InvariantCulture) + "  "
                + (string.IsNullOrWhiteSpace(item.Title) ? "Untitled event" : item.Title);
            savedRow.SetActive(true);
            return;
        }

        GameObject row = new GameObject(
            "Calendar Item " + item.Title,
            typeof(RectTransform),
            typeof(LayoutElement));
        row.layer = 5;
        row.transform.SetParent(listContent, false);
        row.GetComponent<LayoutElement>().preferredHeight = 50f;

        Image dot = CreateImage("Type", row.transform, item.IsManual ? Red : Green);
        dot.sprite = circleSprite;
        SetAnchors(dot.rectTransform, 0f, 0.20f, 0.07f, 0.80f);

        string title = string.IsNullOrWhiteSpace(item.Title) ? "Untitled event" : item.Title;
        Text label = CreateText(
            "Title",
            row.transform,
            item.Date.ToString("MMM d", CultureInfo.InvariantCulture) + "  " + title,
            22,
            Navy,
            TextAnchor.MiddleLeft,
            FontStyle.Normal);
        SetAnchors(label.rectTransform, 0.09f, 0f, 1f, 1f);
    }

    private void ClearList()
    {
        if (listContent == null)
        {
            return;
        }

        for (int index = listContent.childCount - 1; index >= 0; index--)
        {
            GameObject child = listContent.GetChild(index).gameObject;
            child.SetActive(false);
            Destroy(child);
        }
    }

    private DateTime DefaultCreationDate()
    {
        DateTime today = DateTime.Today;
        if (today.Year == displayedMonth.Year && today.Month == displayedMonth.Month)
        {
            return today;
        }

        return displayedMonth;
    }

    private void OpenCreateForDate(DateTime date)
    {
        pendingManualDate = date.Date;
        eventScreen.OpenCalendarEventCreate(date.Date);
    }

    private void SubmitPendingManualEvent(eventgetter owner)
    {
        if (owner == null || !pendingManualDate.HasValue)
        {
            return;
        }

        string title = owner.Eventname.text.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            ShowPopup(owner, "Uh oh!", "An event name is required to add a calendar event.");
            return;
        }

        owner.save.interactable = false;
        if (owner.Saveloader != null)
        {
            owner.Saveloader.SetActive(true);
        }

        StartCoroutine(CreateManualEvent(
            owner,
            title,
            pendingManualDate.Value,
            owner.cityname.text.Trim(),
            string.IsNullOrWhiteSpace(owner.typegetter) ? "Other" : owner.typegetter));
    }

    private IEnumerator CreateManualEvent(
        eventgetter owner,
        string title,
        DateTime date,
        string description,
        string eventType)
    {
        JObject body = new JObject
        {
            ["title"] = title,
            ["date"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["description"] = description,
            ["eventType"] = eventType
        };

        using (UnityWebRequest request = new UnityWebRequest(ApiBaseUrl() + "/v1/calendar", "POST"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + apigetter.jwt);
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body.ToString()));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            owner.save.interactable = true;
            if (owner.Saveloader != null)
            {
                owner.Saveloader.SetActive(false);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Calendar event create failed (" + request.responseCode + "): " + request.downloadHandler.text);
                ShowPopup(owner, "Uh oh!", "The calendar event could not be saved. Please try again.");
                yield break;
            }
        }

        pendingManualDate = null;
        owner.Eventname.text = string.Empty;
        owner.cityname.text = string.Empty;
        owner.typegetter = string.Empty;
        owner.anim.Play("event off");
        owner.closeevent();
        owner.screencurenter(ScreenIndex);
    }

    private string ApiBaseUrl()
    {
        string baseUrl = eventScreen != null ? eventScreen.baseurl : null;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            baseUrl = baselink.Url;
        }

        return baseUrl.TrimEnd('/');
    }

    private static bool TryParseCalendarDate(string value, out DateTime date)
    {
        date = default(DateTime);
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        string[] formats =
        {
            "M/d/yyyy",
            "MM/dd/yyyy",
            "yyyy-MM-dd",
            "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK"
        };

        if (DateTime.TryParseExact(
                value,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal,
                out date))
        {
            return true;
        }

        return DateTime.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal,
            out date);
    }

    private static void ShowPopup(eventgetter owner, string heading, string message)
    {
        owner.popup.SetActive(true);
        owner.popuppanel.pp.Play("popp");
        owner.popuppanel.header.text = heading;
        owner.popuppanel.description.text = message;
        owner.UpgradeBtn.SetActive(false);
    }

    private Image CreateImage(string objectName, Transform parent, Color color)
    {
        GameObject imageObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image));
        imageObject.layer = 5;
        imageObject.transform.SetParent(parent, false);
        Image image = imageObject.GetComponent<Image>();
        image.color = color;
        return image;
    }

    private Text CreateText(
        string objectName,
        Transform parent,
        string value,
        int size,
        Color color,
        TextAnchor alignment,
        FontStyle style)
    {
        GameObject textObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Text));
        textObject.layer = 5;
        textObject.transform.SetParent(parent, false);
        Text text = textObject.GetComponent<Text>();
        text.font = font;
        text.text = value;
        text.fontSize = size;
        text.color = color;
        text.alignment = alignment;
        text.fontStyle = style;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = Math.Max(12, size / 2);
        text.resizeTextMaxSize = size;
        return text;
    }

    private Button CreateCircleButton(
        string objectName,
        Transform parent,
        string labelValue,
        Color color,
        int fontSize)
    {
        GameObject buttonObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));
        buttonObject.layer = 5;
        buttonObject.transform.SetParent(parent, false);

        Image image = buttonObject.GetComponent<Image>();
        image.sprite = circleSprite;
        image.color = color;

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;

        Text label = CreateText(
            "Label",
            buttonObject.transform,
            labelValue,
            fontSize,
            Color.white,
            TextAnchor.MiddleCenter,
            FontStyle.Bold);
        Stretch(label.rectTransform);
        label.raycastTarget = false;
        return button;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void SetAnchors(
        RectTransform rect,
        float minX,
        float minY,
        float maxX,
        float maxY)
    {
        rect.anchorMin = new Vector2(minX, minY);
        rect.anchorMax = new Vector2(maxX, maxY);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static Sprite CreateCircleSprite()
    {
        const int size = 64;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.name = "Calendar Circle";
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        Color[] pixels = new Color[size * size];
        float radius = (size - 1) * 0.5f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float alpha = Mathf.Clamp01(radius - distance + 1f);
                pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return Sprite.Create(
            texture,
            new Rect(0f, 0f, size, size),
            new Vector2(0.5f, 0.5f),
            size);
    }

    private static Color Hex(string value)
    {
        ColorUtility.TryParseHtmlString(value, out Color color);
        return color;
    }

    private sealed class DayCell
    {
        public DayCell(Button button, Text label, Image background)
        {
            Button = button;
            Label = label;
            Background = background;
        }

        public Button Button { get; private set; }
        public Text Label { get; private set; }
        public Image Background { get; private set; }
    }

    private sealed class CalendarItem
    {
        public bool IsManual { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}
