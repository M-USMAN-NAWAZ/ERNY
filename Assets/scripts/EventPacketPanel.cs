using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class EventPacketPanel : MonoBehaviour
{
    public enum Mode { Create, Edit, Card }
    private static readonly string[] Fields = {
        "flight", "car-rental", "lodging", "train", "waiver", "event-guide",
        "shuttle", "bib", "custom1", "custom2", "custom3", "custom4"
    };
    private static readonly string[] Labels = {
        "Flight", "Car Rental", "Lodging", "Train", "Waiver", "Event Guide",
        "Shuttle", "Bib", "Custom 1", "Custom 2", "Custom 3", "Custom 4"
    };
    private static readonly int[] LinkIcons = { 8, 9, 10, -1, -1, 12, 9, 13, -1, -1, -1, -1 };
    private static readonly Color Navy = new Color32(43, 53, 91, 255);
    private static readonly Color Green = new Color32(186, 210, 157, 255);
    private static readonly Color Red = new Color32(177, 73, 87, 255);
    private static readonly Dictionary<string, Dictionary<string, string>> Retry =
        new Dictionary<string, Dictionary<string, string>>();
    private readonly Dictionary<string, string> draft = new Dictionary<string, string>();
    private readonly Dictionary<string, EventPacketApi.Document> documents = new Dictionary<string, EventPacketApi.Document>();
    [SerializeField] private Button[] slots = new Button[Fields.Length];
    [SerializeField] private Image[] slotArt = new Image[Fields.Length];
    [SerializeField] private Sprite[] emptySprites, filledSprites, selectedSprites;
    [SerializeField] private Mode mode;
    [SerializeField] private Text status, fileName;
    [SerializeField] private InputField rename;
    [SerializeField] private Button open, choose, saveName, remove;
    [SerializeField] private Button packetHeader;
    [SerializeField] private RectTransform packetBody;
    [SerializeField] private float headerHeight, expandedHeight;
    [SerializeField] private Color emptySlotColor = Green;
    [SerializeField] private Color filledSlotColor = Navy;
    [SerializeField] private Color selectedSlotColor = Red;
    private static Sprite circle;
    private Font font;
    private string eventId;
    private int selected = -1, revision;
    private bool loadNeeded, busy, controlsBound;

    internal static EventPacketPanel Attach(ScrollRect scroll, Mode view, Selectable[] sourceIcons, string id = null)
    {
        if (scroll == null || scroll.content == null) return null;
        var existing = FindPanel(scroll);
        if (!EventPacketApi.Enabled)
        {
            if (existing != null) existing.gameObject.SetActive(false);
            return null;
        }
        var panel = existing != null ? existing : CreateEditable(scroll, view, sourceIcons, null);
        if (panel == null) return null;
        panel.BindControls();
        panel.Bind(id);
        return panel;
    }

    public static EventPacketPanel FindPanel(ScrollRect scroll) =>
        scroll != null && scroll.content != null
            ? scroll.content.GetComponentsInChildren<EventPacketPanel>(true)
                .FirstOrDefault(panel => panel.transform.parent == scroll.content)
            : null;

    public static bool HasLayout(ScrollRect scroll) => GalleryHeading(scroll) != null;

    private static Text GalleryHeading(ScrollRect scroll)
    {
        var gallery = scroll != null && scroll.content != null ? scroll.content.Find("Gallery") : null;
        return gallery != null ? gallery.GetComponentsInChildren<Text>(true)
            .FirstOrDefault(t => string.Equals(t.text.Trim(), "Gallery", StringComparison.OrdinalIgnoreCase)) : null;
    }

    public static EventPacketPanel CreateEditable(ScrollRect scroll, Mode view, Selectable[] sourceIcons, Sprite iconBackground)
    {
        var existing = FindPanel(scroll);
        if (existing != null) return existing;
        var galleryHeader = GalleryHeading(scroll);
        if (galleryHeader == null) return null;
        var content = scroll.content;
        var gallery = content.Find("Gallery");
        var root = new GameObject("Event Packet (" + view + ")", typeof(RectTransform), typeof(EventPacketPanel));
        root.layer = content.gameObject.layer;
        root.transform.SetParent(content, false);
        root.transform.SetSiblingIndex(gallery.GetSiblingIndex());
        var panel = root.GetComponent<EventPacketPanel>();
        panel.mode = view;
        panel.font = galleryHeader.font;
        panel.Build(galleryHeader.fontSize, sourceIcons, iconBackground != null ? iconBackground : CreateIconBackground());
        panel.MatchLinksLayout(scroll, sourceIcons);
        panel.Refresh();
        root.SetActive(false);
        return panel;
    }

    public bool HasLinksLayout => packetBody != null && packetHeader != null;

    public bool HasStateSprites => emptySprites?.Length == Fields.Length &&
        filledSprites?.Length == Fields.Length && selectedSprites?.Length == Fields.Length;

    public void SetSlotSprites(Sprite[] empty, Sprite[] filled, Sprite[] selectedArt)
    {
        emptySprites = empty;
        filledSprites = filled;
        selectedSprites = selectedArt;
        for (int i = 0; i < slotArt.Length; i++)
        {
            var art = slotArt[i];
            art.sprite = emptySprites[i];
            art.color = Color.white;
            var oldIcon = art.transform.Find("Icon");
            if (oldIcon != null) oldIcon.gameObject.SetActive(false);
            var oldCaption = art.transform.Find("DOC");
            if (oldCaption != null) oldCaption.gameObject.SetActive(false);
        }
    }

    public bool MatchLinksLayout(ScrollRect scroll, Selectable[] sourceIcons, bool recordUndo = false)
    {
        if (HasLinksLayout) return false;
        var sourceGrid = sourceIcons?.Where(icon => icon != null)
            .Select(icon => icon.GetComponentInParent<GridLayoutGroup>(true))
            .FirstOrDefault(grid => grid != null && grid.transform.IsChildOf(scroll.content));
        if (sourceGrid == null)
            sourceGrid = scroll.content.GetComponentsInChildren<GridLayoutGroup>(true)
                .FirstOrDefault(grid => grid.GetComponentsInChildren<Selectable>(true).Length >= 20);
        var sourceTitle = LinksHeading(scroll.content);
        var title = transform.Find("Packet header") as RectTransform;
        if (title == null || slots.Any(slot => slot == null)) return false;

        Vector2 cell = sourceGrid != null ? sourceGrid.cellSize : new Vector2(160, 160);
        Vector2 spacing = sourceGrid != null ? sourceGrid.spacing : new Vector2(95, 83.1f);
        Transform sourceSection = sourceTitle != null ? sourceTitle.transform : null;
        while (sourceSection != null && sourceSection.parent != scroll.content)
            sourceSection = sourceSection.parent;
        float authoredHeight = sourceSection is RectTransform section ? section.rect.height : 0;
        headerHeight = authoredHeight > 0 ? authoredHeight : 132;

        var titleImage = title.GetComponent<Image>();
        var titleText = title.GetComponentInChildren<Text>(true);
        if (sourceTitle != null)
        {
            titleText.font = sourceTitle.font;
            titleText.fontStyle = sourceTitle.fontStyle;
            titleText.fontSize = sourceTitle.fontSize;
            titleText.color = sourceTitle.color;
            titleText.resizeTextForBestFit = sourceTitle.resizeTextForBestFit;
            titleText.resizeTextMinSize = sourceTitle.resizeTextMinSize;
            titleText.resizeTextMaxSize = sourceTitle.resizeTextMaxSize;
            var sourceImage = sourceTitle.GetComponentInParent<Image>(true);
            if (sourceImage != null && sourceSection != null && sourceImage.transform.IsChildOf(sourceSection))
            {
                titleImage.sprite = sourceImage.sprite;
                titleImage.type = sourceImage.type;
                titleImage.color = sourceImage.color;
            }
        }
        PlaceFromTop(title, 0, 1, 0, headerHeight);
        packetHeader = title.GetComponent<Button>();
        if (packetHeader == null)
        {
#if UNITY_EDITOR
            if (recordUndo) packetHeader = UnityEditor.Undo.AddComponent<Button>(title.gameObject);
            else
#endif
                packetHeader = title.gameObject.AddComponent<Button>();
        }
        packetHeader.targetGraphic = titleImage;

        var tip = transform.GetComponentsInChildren<Text>(true).FirstOrDefault(text =>
            text.transform.parent == transform && text.text.StartsWith("Tap ", StringComparison.Ordinal));
        packetBody = Rect("Packet Body", transform, 0, 0, 1, 1);
        TrackCreated(packetBody, recordUndo);
        float tipHeight = mode == Mode.Card ? 106 : 150;
        float gridHeight = cell.y * 3 + spacing.y * 2;
        float detailsTop = tipHeight + gridHeight + 32;
        expandedHeight = headerHeight + detailsTop + (mode == Mode.Card ? 145 : 310);
        PlaceFromTop(packetBody, 0, 1, headerHeight, expandedHeight - headerHeight);

        if (tip != null)
        {
            Move(tip.transform, packetBody, recordUndo);
            PlaceFromTop(tip.rectTransform, .08f, .92f, 12, tipHeight - 24);
        }
        var gridRoot = Rect("Packet Icon Grid", packetBody, 0, 0, 1, 1);
        TrackCreated(gridRoot, recordUndo);
        PlaceFromTop(gridRoot, 0, 1, tipHeight, gridHeight);
        var grid = gridRoot.gameObject.AddComponent<GridLayoutGroup>();
        grid.cellSize = cell;
        grid.spacing = spacing;
        grid.childAlignment = TextAnchor.UpperCenter;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        for (int i = 0; i < slots.Length; i++)
        {
            Move(slots[i].transform, gridRoot, recordUndo);
            var caption = slots[i].transform.Find(Labels[i]);
            if (caption != null) caption.gameObject.SetActive(false);
            Stretch(slotArt[i].rectTransform, 0, 0, 1, 1);
        }
        PlaceControl(fileName.rectTransform, .06f, .94f, detailsTop, 58, recordUndo);
        PlaceControl((RectTransform)rename.transform, .07f, .93f, detailsTop + 68, 70, recordUndo);
        PlaceControl((RectTransform)open.transform, .05f, .26f, detailsTop + 152, 66, recordUndo);
        PlaceControl((RectTransform)choose.transform, .28f, .49f, detailsTop + 152, 66, recordUndo);
        PlaceControl((RectTransform)saveName.transform, .51f, .72f, detailsTop + 152, 66, recordUndo);
        PlaceControl((RectTransform)remove.transform, .74f, .95f, detailsTop + 152, 66, recordUndo);
        PlaceControl(status.rectTransform, .05f, .95f, detailsTop + (mode == Mode.Card ? 60 : 222), 82, recordUndo);
        SetExpanded(true);
        return true;
    }

    private static Text LinksHeading(Transform content) => content.GetComponentsInChildren<Text>(true)
        .FirstOrDefault(label => string.Equals(label.text.Trim(), "Links", StringComparison.OrdinalIgnoreCase));

    private void PlaceControl(RectTransform element, float left, float right, float top, float height, bool recordUndo)
    {
        Move(element, packetBody, recordUndo);
        PlaceFromTop(element, left, right, top, height);
    }

    private static void Move(Transform element, Transform parent, bool recordUndo)
    {
#if UNITY_EDITOR
        if (recordUndo) { UnityEditor.Undo.SetTransformParent(element, parent, "Match Packet to Links"); return; }
#endif
        element.SetParent(parent, false);
    }

    private static void TrackCreated(Transform element, bool recordUndo)
    {
#if UNITY_EDITOR
        if (recordUndo) UnityEditor.Undo.RegisterCreatedObjectUndo(element.gameObject, "Match Packet to Links");
#endif
    }

    private static void PlaceFromTop(RectTransform element, float left, float right, float top, float height)
    {
        element.anchorMin = new Vector2(left, 1);
        element.anchorMax = new Vector2(right, 1);
        element.offsetMin = new Vector2(0, -top - height);
        element.offsetMax = new Vector2(0, -top);
    }

    private static void Stretch(RectTransform element, float left, float bottom, float right, float top)
    {
        element.anchorMin = new Vector2(left, bottom);
        element.anchorMax = new Vector2(right, top);
        element.offsetMin = element.offsetMax = Vector2.zero;
    }

    private void SetExpanded(bool expanded)
    {
        packetBody.gameObject.SetActive(expanded);
        var root = (RectTransform)transform;
        float height = expanded ? expandedHeight : headerHeight;
        float change = height - root.rect.height;
        root.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        root.anchoredPosition -= Vector2.up * (change * (1 - root.pivot.y));
        if (root.TryGetComponent<LayoutElement>(out var layout)) layout.preferredHeight = height;
        if (root.parent is RectTransform parent)
        {
            if (root.gameObject.activeInHierarchy) LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
            else LayoutRebuilder.MarkLayoutForRebuild(parent);
        }
    }

    private void KeepOnlyPacketIcons()
    {
        fileName.gameObject.SetActive(false);
        open.gameObject.SetActive(false);
        choose.gameObject.SetActive(false);
        remove.gameObject.SetActive(false);
        saveName.gameObject.SetActive(false);
        rename.gameObject.SetActive(false);
        if (!HasLinksLayout) return;

        // An authored header can wrap the actual Button; anchor the whole header, not just its label.
        var header = (RectTransform)packetHeader.transform;
        while (header.parent != transform && header.parent is RectTransform parent)
            header = parent;
        if (header.parent == transform)
        {
            var buttonRect = (RectTransform)packetHeader.transform;
            if (buttonRect != header && header.TryGetComponent<Image>(out _) &&
                buttonRect.TryGetComponent<Image>(out var extraYellow))
            {
                // Keep the nested button as the hit area, but draw only the full-width header.
                extraYellow.color = Color.clear;
                packetHeader.transition = Selectable.Transition.None;
                if (buttonRect.parent == header) Stretch(buttonRect, 0, 0, 1, 1);
            }
            var root = (RectTransform)transform;
            float top = root.rect.yMax - (header.localPosition.y + header.rect.yMax);
            float height = header.rect.height;
            if (root.parent is RectTransform content && TryGetLinksHeaderSpacing(content, out var rowHeight,
                out var topInset, out var visibleHeight))
            {
                headerHeight = rowHeight;
                top = topInset;
                height = visibleHeight;
            }
            var position = header.anchoredPosition;
            var size = header.sizeDelta;
            header.anchorMin = new Vector2(header.anchorMin.x, 1);
            header.anchorMax = new Vector2(header.anchorMax.x, 1);
            header.sizeDelta = new Vector2(size.x, height);
            header.anchoredPosition = new Vector2(position.x, -top - height * (1 - header.pivot.y));
            PlaceFromTop(packetBody, packetBody.anchorMin.x, packetBody.anchorMax.x, headerHeight, packetBody.rect.height);
        }

        if (packetBody.Find("Packet Icon Grid") is RectTransform grid)
        {
            float statusTop = packetBody.rect.yMax - grid.localPosition.y - grid.rect.yMin + 15;
            PlaceFromTop(status.rectTransform, .05f, .95f, statusTop, 70);
            expandedHeight = headerHeight + statusTop + 85;
        }
    }

    private static bool TryGetLinksHeaderSpacing(RectTransform content, out float rowHeight, out float top, out float visibleHeight)
    {
        rowHeight = top = visibleHeight = 0;
        var title = LinksHeading(content);
        var artwork = title != null ? title.GetComponentInParent<Image>(true)?.rectTransform : null;
        if (artwork == null) return false;
        var row = artwork;
        while (row.parent != content && row.parent is RectTransform parent) row = parent;
        if (row.parent != content || row == artwork) return false;
        float artworkTop = row.InverseTransformPoint(artwork.TransformPoint(Vector3.up * artwork.rect.yMax)).y;
        float artworkBottom = row.InverseTransformPoint(artwork.TransformPoint(Vector3.up * artwork.rect.yMin)).y;
        rowHeight = row.rect.height;
        top = row.rect.yMax - artworkTop;
        visibleHeight = artworkTop - artworkBottom;
        return rowHeight > 0 && top >= 0 && visibleHeight > 0 && top + visibleHeight <= rowHeight + .01f;
    }

    private string RetryKey => apigetter.id + "/" + eventId;

    private void Bind(string id)
    {
        if (mode == Mode.Create) { gameObject.SetActive(true); Refresh(); return; }
        if (string.IsNullOrEmpty(id)) { gameObject.SetActive(false); return; }
        if (eventId != id)
        {
            revision++;
            StopAllCoroutines();
            busy = false;
            documents.Clear();
            selected = -1;
        }
        eventId = id;
        loadNeeded = true;
        gameObject.SetActive(true);
        Refresh();
    }

    private void OnEnable() => loadNeeded = true;

    private void Start()
    {
        if (!EventPacketApi.Enabled || !controlsBound) gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        revision++;
        busy = false;
    }

    private void Update()
    {
        if (mode == Mode.Create || !loadNeeded || busy || string.IsNullOrEmpty(eventId)) return;
        loadNeeded = false;
        Run("GET", null, null, null);
    }

    private void Build(int headingSize, Selectable[] sourceIcons, Sprite iconBackground)
    {
        int size = Mathf.Max(headingSize, 30);
        ((RectTransform)transform).sizeDelta = new Vector2(0, 12.4f * size);
        var background = gameObject.AddComponent<Image>();
        background.color = Color.white;
        background.raycastTarget = false;
        var header = Rect("Packet header", transform, 0, .89f, 1, 1);
        header.gameObject.AddComponent<Image>().color = new Color32(247, 197, 88, 255);
        Label(header, "PACKET", size, 0, 0, 1, 1);
        Label(transform, mode == Mode.Card ? "Tap a filled slot to open a document." : "Tap an icon to add a race document.",
            size * 2 / 3, .04f, .80f, .96f, .88f);

        for (int i = 0; i < slots.Length; i++)
        {
            float x = .06f + (i % 4) * .225f, y = .64f - (i / 4) * .13f;
            var button = MakeButton(transform, Labels[i], size * 2 / 3, x, y, x + .205f, y + .117f, out var caption);
            slots[i] = button;
            button.image.color = Color.clear;
            button.transition = Selectable.Transition.None;
            caption.rectTransform.anchorMax = new Vector2(1, .32f);
            var art = Rect("Packet icon", button.transform, .20f, .34f, .80f, 1).gameObject.AddComponent<Image>();
            art.sprite = iconBackground;
            art.preserveAspect = true;
            art.raycastTarget = false;
            slotArt[i] = art;
            int index = LinkIcons[i];
            if (sourceIcons != null && index >= 0 && index < sourceIcons.Length &&
                sourceIcons[index] != null && sourceIcons[index].targetGraphic is Image icon && icon.sprite != null)
            {
                var picture = Rect("Icon", art.transform, .25f, .25f, .75f, .75f).gameObject.AddComponent<Image>();
                picture.sprite = icon.sprite;
                picture.preserveAspect = true;
                picture.raycastTarget = false;
            }
            else Label(art.transform, "DOC", size / 2, 0, 0, 1, 1).color = Color.white;
        }

        fileName = Label(transform, "", size * 2 / 3, .05f, .30f, .95f, .37f);
        fileName.name = "Selected File";
        status = Label(transform, "", size * 2 / 3, .05f, .015f, .95f, .095f);
        status.name = "Packet Status";
        open = MakeButton(transform, "Open", size * 2 / 3, .05f, .10f, .26f, .18f, out _);
        choose = MakeButton(transform, "Replace", size * 2 / 3, .28f, .10f, .49f, .18f, out _);
        saveName = MakeButton(transform, "Rename", size * 2 / 3, .51f, .10f, .72f, .18f, out _);
        remove = MakeButton(transform, "Remove", size * 2 / 3, .74f, .10f, .95f, .18f, out _);
        var input = Rect("Document name", transform, .07f, .20f, .93f, .29f);
        input.gameObject.AddComponent<Image>().color = new Color32(235, 239, 227, 255);
        rename = input.gameObject.AddComponent<InputField>();
        rename.textComponent = Label(input, "", size * 2 / 3, .03f, .05f, .97f, .95f);
        rename.lineType = InputField.LineType.SingleLine;
        rename.characterLimit = 120;
    }

    private void BindControls()
    {
        if (controlsBound) return;
        controlsBound = true;
        KeepOnlyPacketIcons();
        if (HasLinksLayout)
        {
            packetHeader.onClick.AddListener(() => SetExpanded(!packetBody.gameObject.activeSelf));
            SetExpanded(false);
        }
        for (int i = 0; i < slots.Length; i++)
        {
            int slot = i;
            slots[i].onClick.AddListener(() => Select(slot));
        }
    }

    private void Select(int slot)
    {
        if (busy) return;
        selected = slot;
        status.text = "";
        Refresh();
        if (mode == Mode.Card) OpenSelected();
        else if (mode == Mode.Edit && !documents.ContainsKey(Fields[slot]) &&
            Retry.TryGetValue(RetryKey, out var queued) && queued.TryGetValue(Fields[slot], out var pending))
            Run("POST", Fields[slot], pending, null);
        else Pick();
    }

    private void Pick()
    {
        if (busy || selected < 0) return;
        if (ApiRequestGenerator.guestlogin != 0 || string.IsNullOrEmpty(apigetter.jwt))
        {
            status.text = "Sign in on staging to add race documents.";
            return;
        }
        int stamp = revision;
        string field = Fields[selected];
        busy = true;
        Refresh();
        EventPacketPicker.Pick((path, error) =>
        {
            if (this == null || stamp != revision)
            {
                EventPacketPicker.Release(path);
                return;
            }
            busy = false;
            if (path == null) { selected = -1; status.text = error ?? ""; Refresh(); return; }
            if (mode == Mode.Create)
            {
                if (draft.TryGetValue(field, out var previous) && previous != path) EventPacketPicker.Release(previous);
                draft[field] = path;
                selected = -1;
                status.text = "";
                Refresh();
            }
            else
            {
                if (!Retry.TryGetValue(RetryKey, out var queued)) Retry[RetryKey] = queued = new Dictionary<string, string>();
                if (queued.TryGetValue(field, out var prior) && prior != path) EventPacketPicker.Release(prior);
                queued[field] = path;
                Run(documents.ContainsKey(field) ? "PATCH" : "POST", field, path, null);
            }
        });
    }

    private void OpenSelected()
    {
        if (selected >= 0 && documents.TryGetValue(Fields[selected], out var doc) &&
            Uri.TryCreate(doc.url, UriKind.Absolute, out var url) && url.Scheme == Uri.UriSchemeHttps)
            EventPacketPreview.Open(url, font != null ? font : status.font);
    }

    private void Run(string method, string field, string path, string name)
    {
        if (busy || !isActiveAndEnabled) return;
        busy = true;
        int stamp = revision;
        string key = RetryKey;
        status.text = "";
        Refresh();
        StartCoroutine(EventPacketApi.Request(method, eventId, field, path, name, (list, error) =>
        {
            if (stamp != revision) return;
            busy = false;
            if (method != "GET") selected = -1;
            if (list != null)
            {
                documents.Clear();
                foreach (var item in list)
                    if (item != null && !string.IsNullOrEmpty(item.field)) documents[item.field] = item;
                if (field != null && path != null) Forget(key, field);
            }
            status.text = error ?? (Retry.ContainsKey(key) ? (mode == Mode.Card
                ? "Some files are waiting to upload. Open Edit Event to retry."
                : "Some files are waiting to upload. Tap the slot to retry.") : "");
            Refresh();
        }));
    }

    private static void Forget(string key, string field)
    {
        if (!Retry.TryGetValue(key, out var queued) || !queued.TryGetValue(field, out var path)) return;
        EventPacketPicker.Release(path);
        queued.Remove(field);
        if (queued.Count == 0) Retry.Remove(key);
    }

    internal IEnumerator UploadDraft(string id)
    {
        if (draft.Count == 0 || string.IsNullOrWhiteSpace(id)) yield break;
        string key = apigetter.id + "/" + id;
        var queued = new Dictionary<string, string>(draft);
        Retry[key] = queued;
        draft.Clear();
        foreach (var item in queued.ToArray())
        {
            yield return EventPacketApi.Request("POST", id, item.Key, item.Value, null, (documents, error) =>
            {
                if (error == null) Forget(key, item.Key);
                else Debug.LogWarning("Race event saved; a Packet upload needs retry in Edit Event: " + error);
            });
        }
        Refresh();
    }

    internal void ClearDraft()
    {
        foreach (var path in draft.Values) EventPacketPicker.Release(path);
        draft.Clear();
        selected = -1;
        status.text = "";
        Refresh();
    }

    private void OnDestroy()
    {
        foreach (var path in draft.Values) EventPacketPicker.Release(path);
    }

    private void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool filled = documents.ContainsKey(Fields[i]) || draft.ContainsKey(Fields[i]);
            var art = slotArt[i];
            if (HasStateSprites)
            {
                art.overrideSprite = i == selected ? selectedSprites[i] : filled ? filledSprites[i] : emptySprites[i];
                art.color = Color.white;
            }
            else art.color = i == selected ? selectedSlotColor : filled ? filledSlotColor : emptySlotColor;
            slots[i].interactable = !busy && (mode != Mode.Card || filled);
        }
    }

    public static Sprite CreateIconBackground()
    {
        if (circle != null) return circle;
        const int size = 48;
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        var pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float radius = new Vector2(x + .5f - size / 2f, y + .5f - size / 2f).magnitude;
            pixels[y * size + x] = new Color32(255, 255, 255,
                (byte)Mathf.RoundToInt(255 * Mathf.Clamp01(size / 2f - radius)));
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        texture.name = "Packet Icon Background";
        circle = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
        circle.name = "Packet Circle";
        return circle;
    }

    private RectTransform Rect(string name, Transform parent, float x, float y, float x2, float y2)
    {
        var child = new GameObject(name, typeof(RectTransform));
        child.layer = gameObject.layer;
        var rect = (RectTransform)child.transform;
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(x, y);
        rect.anchorMax = new Vector2(x2, y2);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        return rect;
    }

    private Text Label(Transform parent, string value, int size, float x, float y, float x2, float y2)
    {
        var label = Rect(value, parent, x, y, x2, y2).gameObject.AddComponent<Text>();
        label.font = font;
        label.text = value;
        label.fontSize = size;
        label.resizeTextForBestFit = true;
        label.resizeTextMinSize = Mathf.Max(8, size / 2);
        label.resizeTextMaxSize = size;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Navy;
        label.supportRichText = label.raycastTarget = false;
        return label;
    }

    private Button MakeButton(Transform parent, string caption, int size, float x, float y, float x2, float y2, out Text label)
    {
        var rect = Rect(caption, parent, x, y, x2, y2);
        var image = rect.gameObject.AddComponent<Image>();
        image.color = Green;
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        label = Label(rect, caption, size, .04f, .03f, .96f, .97f);
        return button;
    }
}
