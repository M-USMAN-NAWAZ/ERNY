using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

// RunSignup handles registration; Erny saves the race separately.
public sealed class RaceDiscoveryController : MonoBehaviour
{
    [Serializable] public sealed class SearchFilters
    {
        public string name, raceType, country, state, zip, radiusMiles, minDistance, maxDistance, unit, fromMonth, toMonth;
    }
    [Serializable] public sealed class RaceResult
    {
        public string id, name, date, location, distances, url, logoUrl;
        public DateTime sortDate;
        public Sprite logo;
        public RaceEntry[] entries;
    }

    public sealed class RaceEntry
    {
        public string name, distance, startTime;
        public DateTime date;
    }

    public static RaceDiscoveryController Instance { get; private set; }
    public UnityEvent searchRequested = new UnityEvent();
    public SearchFilters CurrentFilters { get; private set; }
    public event Action<SearchFilters> SearchRequested;
    public event Action<RaceResult> RaceSelected;
    [SerializeField] private GameObject filtersPanel, resultsPanel;
    [SerializeField] private InputField homeSearch, raceName, resultsSearch, zip, minDistance, maxDistance;
    [SerializeField] private Dropdown state;
    [SerializeField] private Dropdown raceType, radius, country, fromMonth, toMonth;
    [SerializeField] private Toggle metric;
    [SerializeField] private Button filterButton, closeFilters, startButton, closeResults, editFilters, clearFilters;
    [SerializeField] private Text filterCount, status;
    [SerializeField] private RectTransform resultsContent;
    [SerializeField] private GameObject rowTemplate;
    [SerializeField] private Font bodyFont, headingFont;
    [SerializeField] private Sprite fieldSprite;
    [SerializeField] private Button nextPage, previousPage;
    [SerializeField] private Text pageLabel;
    private readonly List<RaceResult> races = new List<RaceResult>();
    private readonly List<GameObject> rows = new List<GameObject>();
    private bool bound, searching;
    private int page = 1;
    private UnityWebRequest activeRequest;
    private readonly List<UnityEngine.Object> logoAssets = new List<UnityEngine.Object>();
    private static readonly Color Navy = new Color32(43, 53, 89, 255);
    private static readonly Color Red = new Color32(181, 81, 96, 255);
    private static readonly Color Green = new Color32(143, 170, 111, 255);
    private eventgetter owner;
    private readonly RaceWebView browser = new RaceWebView();
    private readonly Vector3[] webCorners = new Vector3[4];
    [SerializeField] private GameObject racePanel, confirmation;
    [SerializeField] private RectTransform webArea, raceActions;
    private Vector2 raceActionsRestingPosition;
    [SerializeField] private Text raceAction, confirmationStatus, browserStatus;
    [SerializeField] private Sprite addedRaceIcon;
    [SerializeField] private Button raceBackButton, raceRegisterButton, checkoutBackButton, confirmRegistration;
    [SerializeField] private Dropdown eventChoice;
    [SerializeField] private InputField finishTime;
    private RaceResult selectedRace;
    private RaceEntry[] entries;
    private Uri raceAddress;
    private bool savingRace, browserAvailable, racePageBound;
    private Image raceRegisterIcon;
    private bool originalIconAspect, raceAdded;

    public static void EnsureCreated(eventgetter owner)
    {
        if (owner == null || owner.canv == null || owner.panels.Length == 0) return;
        var controller = owner.panels[0].GetComponentInChildren<RaceDiscoveryController>(true);
        if (controller == null)
        {
            controller = CreateInterface(owner, owner.name.font, owner.name.font, null);
        }
        Instance = controller;
        controller.owner = owner;
        controller.Bind();
    }

    public static RaceDiscoveryController CreateInterface(eventgetter owner, Font body, Font heading, Sprite border,
        Sprite miles = null, Sprite kilometers = null)
    {
        var home = owner.panels[0].transform.Find("Image");
        if (home == null) throw new InvalidOperationException("Home Image was not found.");
        var banner = home.Find("GameObject") as RectTransform;
        if (banner != null)
        {
            Place(banner, 0, .725f, 1, 1);
            if (!banner.TryGetComponent<RectMask2D>(out _))
                banner.gameObject.AddComponent<RectMask2D>();
            foreach (var artwork in banner.GetComponentsInChildren<RawImage>(true))
                artwork.maskable = true;
        }
        if (owner.name != null && owner.name.transform.parent == home)
            Place(owner.name.rectTransform, .076f, .725f, .479f, .785f);
        var c = Box("Race Discovery", home, 0, .1f, 1, .725f, null).AddComponent<RaceDiscoveryController>();
        c.bodyFont = body; c.headingFont = heading; c.fieldSprite = border;
        var title = home.GetComponentsInChildren<Text>(true).FirstOrDefault(t => t.text.Trim() == "Add a race");
        if (title != null) Place((RectTransform)title.transform.parent, 0, .54f, 1, .587f);
        var medals = home.Find("medals") as RectTransform;
        if (medals != null)
        {
            Place(medals, .052f, .116f, .948f, .54f);
            var grid = medals.GetComponent<GridLayoutGroup>();
            if (grid != null) grid.enabled = false;
            int index = 0;
            foreach (RectTransform child in medals)
            {
                if (index >= 9) break;
                int col = index % 3, row = index / 3;
                Place(child, col / 3f + .025f, (2 - row) / 3f + .025f, (col + 1) / 3f - .025f, (3 - row) / 3f - .025f);
                index++;
            }
        }
        c.Label("Discover races", c.transform, .04f, .898f, .96f, .98f, true, Red);
        c.homeSearch = c.Input("Search", c.transform, "Search by name, type or city", .075f, .81f, .74f, .895f);
        c.homeSearch.placeholder.color = Navy;
        c.filterButton = c.Button("Filters", c.transform, "", .778f, .81f, .926f, .895f);
        // Native UI shapes keep the filter icon editable without adding an icon font dependency.
        for (int i = 0; i < 3; i++)
        {
            float y = .23f + i * .25f;
            Box("Line " + i, c.filterButton.transform, .2f, y, .8f, y + .05f, Color.white);
            Box("Slider " + i, c.filterButton.transform, i == 1 ? .57f : .3f, y - .045f, i == 1 ? .68f : .41f, y + .095f, Color.white);
        }
        c.filtersPanel = Box("Filters Panel", c.transform, 0, 0, 1, 1, Color.white);
        var f = c.filtersPanel.transform;
        c.Label("Discover races", f, .08f, .86f, .9f, .99f, true, Red);
        c.closeFilters = c.Button("Close filters", f, "X", .91f, .91f, .98f, .975f);
        c.raceName = c.Input("Race Name", f, "Race Name", .075f, .78f, .925f, .87f);
        c.raceType = c.Drop("Race Type", f, new[] { "All Race Types", "5K", "10K", "15K", "Half Marathon", "Marathon", "Trail", "Obstacle", "Fun Run", "Other" }, .075f, .665f, .925f, .755f);
        c.radius = c.Drop("Radius", f, new[] { "5 Miles", "10 Miles", "25 Miles", "50 Miles", "100 Miles" }, .075f, .55f, .46f, .64f);
        c.Label("from", f, .465f, .55f, .57f, .64f);
        c.zip = c.Input("Zip Code", f, "Zip Code", .58f, .55f, .925f, .64f);
        c.country = c.Drop("Country", f, new[] { "USA", "Canada", "Mexico" }, .075f, .435f, .46f, .525f);
        c.Label("and", f, .465f, .435f, .57f, .525f);
        c.state = c.Drop("State Province", f, new[] { "State/Prov." }, .58f, .435f, .925f, .525f);
        c.minDistance = c.Input("Minimum", f, "Minimum", .075f, .32f, .38f, .41f);
        c.maxDistance = c.Input("Maximum", f, "Maximum", .62f, .32f, .925f, .41f);
        c.minDistance.contentType = c.maxDistance.contentType = InputField.ContentType.DecimalNumber;
        var toggle = Box("Distance Unit", f, .42f, .335f, .58f, .395f, Navy);
        c.metric = toggle.AddComponent<Toggle>();
        var check = Box("Check", toggle.transform, .68f, .2f, .92f, .8f, Green).GetComponent<Image>();
        c.metric.targetGraphic = toggle.GetComponent<Image>(); c.metric.graphic = check;
        if (miles != null && kilometers != null)
        {
            var background = (Image)c.metric.targetGraphic;
            background.sprite = miles; background.color = Color.white; background.preserveAspect = true;
            Place(check.rectTransform, 0, 0, 1, 1);
            check.sprite = kilometers; check.color = Color.white; check.preserveAspect = true; check.raycastTarget = false;
            c.metric.toggleTransition = Toggle.ToggleTransition.None;
            c.metric.SetIsOnWithoutNotify(false);
        }
        else
        {
            var unitLabel = c.Label("mi / km", toggle.transform, .05f, .05f, .68f, .95f, false, Color.white);
            unitLabel.resizeTextMinSize = 18;
        }
        var months = new[] { "Month", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        c.fromMonth = c.Drop("From Month", f, months, .075f, .205f, .46f, .295f);
        c.Label("to", f, .465f, .205f, .57f, .295f);
        c.toMonth = c.Drop("To Month", f, months, .58f, .205f, .925f, .295f);
        c.startButton = c.Button("Start Search", f, "START", .35f, .055f, .65f, .16f);
        c.Label("Currently featuring races across North America.", f, .05f, 0, .95f, .048f).resizeTextMinSize = 16;
        c.resultsPanel = Box("Discover Races Results", owner.canv.transform, 0, .1f, 1, 1, new Color32(243, 244, 245, 255));
        var r = c.resultsPanel.transform;
        c.Label("Discover races", r, .08f, .905f, .87f, .975f, true, Navy).alignment = TextAnchor.MiddleLeft;
        c.closeResults = c.Button("Close results", r, "X", .87f, .94f, .95f, .98f);
        c.editFilters = c.Button("Edit filters", r, "Filters (0)", .08f, .85f, .43f, .9f);
        c.filterCount = c.editFilters.GetComponentInChildren<Text>();
        c.clearFilters = c.Button("Clear filters", r, "X  Clear Filters", .45f, .85f, .81f, .9f);
        c.resultsSearch = c.Input("Results Search", r, "Search by race name, type or city", .08f, .765f, .92f, .83f);
        c.status = c.Label("Search for races to see results here.", r, .08f, .66f, .92f, .75f);
        var scrollObject = Box("Results Scroll", r, .035f, .02f, .965f, .74f, Color.clear);
        scrollObject.AddComponent<RectMask2D>();
        var scroll = scrollObject.AddComponent<ScrollRect>();
        c.resultsContent = (RectTransform)Box("Content", scrollObject.transform, 0, 1, 1, 1, null).transform;
        c.resultsContent.pivot = new Vector2(.5f, 1);
        var layout = c.resultsContent.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 35; layout.padding = new RectOffset(5, 5, 5, 20);
        layout.childControlHeight = layout.childControlWidth = true; layout.childForceExpandHeight = false;
        c.resultsContent.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scroll.content = c.resultsContent; scroll.viewport = (RectTransform)scrollObject.transform;
        scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
        c.rowTemplate = Box("Calendar Independent Race Result Template", c.resultsContent, 0, 0, 1, 1, Color.white);
        c.rowTemplate.name = "Race Result Template";
        c.rowTemplate.AddComponent<LayoutElement>().preferredHeight = 860;
        c.rowTemplate.AddComponent<Button>();
        var logo = Box("Logo", c.rowTemplate.transform, .25f, .4f, .75f, .94f, Color.white).GetComponent<Image>();
        logo.preserveAspect = true;
        c.Label("Race Name", c.rowTemplate.transform, .045f, .27f, .955f, .39f).name = "Name";
        c.Label("Race date", c.rowTemplate.transform, .045f, .17f, .955f, .265f).name = "Date";
        c.Label("City, State", c.rowTemplate.transform, .045f, .08f, .955f, .17f).name = "Location";
        c.Label("Distances", c.rowTemplate.transform, .045f, .015f, .955f, .08f).name = "Distances";
        c.rowTemplate.SetActive(false); c.filtersPanel.SetActive(false); c.resultsPanel.SetActive(false);
        c.CurrentFilters = new SearchFilters();
        return c;
    }

    private void Bind()
    {
        if (bound) return;
        bound = true;
        EnsurePagination();
        EnsureStateDropdown();
        country.onValueChanged.AddListener(_ => RefreshStates());
        filterButton.onClick.AddListener(OpenFilters);
        closeFilters.onClick.AddListener(() => filtersPanel.SetActive(false));
        closeResults.onClick.AddListener(Hide);
        editFilters.onClick.AddListener(OpenFilters);
        startButton.onClick.AddListener(Submit);
        clearFilters.onClick.AddListener(Clear);
        homeSearch.onSubmit.AddListener(value => { raceName.text = value; Submit(); });
        resultsSearch.onSubmit.AddListener(value => { raceName.text = value; Submit(); });
        CurrentFilters = new SearchFilters();
    }

    private void OpenFilters()
    {
        resultsPanel.SetActive(false);
        raceName.text = homeSearch.text;
        filtersPanel.SetActive(true);
    }

    public void Hide()
    {
        CloseRacePage();
        CancelSearch();
        filtersPanel.SetActive(false); resultsPanel.SetActive(false);
    }

    public void Submit()
    {
        CancelSearch();
        page = 1;
        if (!ValidDistance(minDistance.text) || !ValidDistance(maxDistance.text))
        { ShowMessage("Please enter valid positive distances."); return; }
        if (float.TryParse(minDistance.text, NumberStyles.Float, CultureInfo.InvariantCulture, out float min)
            && float.TryParse(maxDistance.text, NumberStyles.Float, CultureInfo.InvariantCulture, out float max) && min > max)
        { ShowMessage("Minimum distance must not exceed maximum distance."); return; }
        if (!string.IsNullOrWhiteSpace(zip.text) && country.value != 0)
        { ShowMessage("ZIP radius search is available for USA races only."); return; }
        if (!string.IsNullOrWhiteSpace(zip.text) && !System.Text.RegularExpressions.Regex.IsMatch(zip.text.Trim(), @"^\d{5}(-\d{4})?$"))
        { ShowMessage("Please enter a valid USA ZIP code."); return; }
        CurrentFilters = new SearchFilters {
            name = raceName.text.Trim(), raceType = raceType.value == 0 ? "" : raceType.captionText.text,
            radiusMiles = string.IsNullOrWhiteSpace(zip.text) ? "" : radius.captionText.text.Split(' ')[0],
            zip = zip.text.Trim(), country = country.value == 0 ? "US" : country.value == 1 ? "CA" : "MX", state = state.value == 0 ? "" : state.captionText.text,
            minDistance = minDistance.text.Trim(), maxDistance = maxDistance.text.Trim(), unit = metric.isOn ? "km" : "mi",
            fromMonth = fromMonth.value == 0 ? "" : fromMonth.value.ToString(CultureInfo.InvariantCulture),
            toMonth = toMonth.value == 0 ? "" : toMonth.value.ToString(CultureInfo.InvariantCulture)
        };
        homeSearch.text = resultsSearch.text = CurrentFilters.name;
        int count = new[] { CurrentFilters.name, CurrentFilters.raceType, CurrentFilters.zip, CurrentFilters.state,
            CurrentFilters.minDistance, CurrentFilters.maxDistance, CurrentFilters.fromMonth, CurrentFilters.toMonth }
            .Count(value => !string.IsNullOrEmpty(value));
        filterCount.text = "Filters (" + count + ")";
        filtersPanel.SetActive(false); resultsPanel.SetActive(true);
        ClearRows();
        searching = true;
        status.text = "Searching races..."; status.gameObject.SetActive(true);
        if (SearchRequested != null || searchRequested.GetPersistentEventCount() > 0)
        { SearchRequested?.Invoke(CurrentFilters); searchRequested.Invoke(); }
        else StartCoroutine(SearchRunSignup());
    }

    public void SetResults(IEnumerable<RaceResult> results)
    {
        searching = false; ClearRows(); races.Clear();
        races.AddRange(results ?? Enumerable.Empty<RaceResult>());
        foreach (var race in races)
        {
            var row = Instantiate(rowTemplate, resultsContent, false); row.name = race.name; rows.Add(row);
            row.transform.Find("Name").GetComponent<Text>().text = race.name;
            row.transform.Find("Date").GetComponent<Text>().text = race.date;
            row.transform.Find("Location").GetComponent<Text>().text = race.location;
            row.transform.Find("Distances").GetComponent<Text>().text = race.distances;
            var logo = row.transform.Find("Logo").GetComponent<Image>();
            logo.sprite = race.logo; logo.enabled = race.logo != null;
            row.GetComponent<Button>().onClick.AddListener(() => {
                if (RaceSelected != null) RaceSelected(race);
                else OpenRacePage(race);
            });
            row.SetActive(true);
        }
        status.text = races.Count == 0 ? "No races found. Try changing your filters." : "";
        status.gameObject.SetActive(races.Count == 0);
        resultsContent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    private void OpenRacePage(RaceResult race)
    {
        if (!Uri.TryCreate(race.url, UriKind.Absolute, out Uri address) || address.Scheme != "https" ||
            !(address.Host == "runsignup.com" || address.Host.EndsWith(".runsignup.com", StringComparison.OrdinalIgnoreCase)))
        {
            ShowMessage("This race doesn't have a supported RunSignup page.");
            return;
        }

        CreateEditableRacePage();
        if (!racePageBound)
        {
            racePageBound = true;
            raceActionsRestingPosition = raceActions.anchoredPosition;
            raceRegisterIcon = raceRegisterButton.transform.Find("Register")?.GetComponent<Image>();
            if (raceRegisterIcon != null) originalIconAspect = raceRegisterIcon.preserveAspect;
#if UNITY_EDITOR
            if (addedRaceIcon == null)
                addedRaceIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(
                    "Assets/UI/NewErny_UI/Discover RunSignup Ui/Asset 8@4x.png");
#endif
            raceBackButton.onClick.AddListener(RaceBack);
            raceRegisterButton.onClick.AddListener(AddRace);
            checkoutBackButton.onClick.AddListener(RaceBack);
            confirmRegistration.onClick.AddListener(() => SaveRace(ChosenEntry));
            if (finishTime != null) finishTime.gameObject.SetActive(false);
            foreach (var label in confirmation.GetComponentsInChildren<Text>(true))
                if (label.text == "Only add this race after completing registration on RunSignup.")
                    label.text = "Choose which RunSignup event to add to Erny.";
        }
        selectedRace = race;
        entries = race.entries != null && race.entries.Length > 0 ? race.entries
            : new[] { new RaceEntry { name = race.name, date = race.sortDate } };
        raceAddress = address;
        SetRaceAdded(entries.Any(entry => eventgetter.HasDiscoveredRace(race, entry)));
        racePanel.transform.SetAsLastSibling();
        racePanel.SetActive(true);
        LoadRaceAddress(address);
    }

    public GameObject CreateEditableRacePage()
    {
        if (racePanel != null) return racePanel;
        var parent = resultsPanel.transform.parent;
        racePanel = Box("RunSignup Race Page", parent, 0, .1004f, 1, 1, Color.white);
        var gold = new Color32(249, 204, 93, 255);
        webArea = (RectTransform)Box("Native Web View", racePanel.transform, 0, 0, 1, 1, null).transform;
        browserStatus = Label("", webArea, .12f, .4f, .88f, .6f);
        browserStatus.alignment = TextAnchor.MiddleCenter;
        var actions = Box("Race Actions", racePanel.transform, .25f, .012f, .75f, .083f, gold);
        raceActions = (RectTransform)actions.transform;
        raceBackButton = Button("Race Back", actions.transform, "<", .05f, .09f, .29f, .91f);
        raceBackButton.GetComponent<Image>().color = Navy;
        Box("Divider", actions.transform, .33f, .15f, .34f, .85f, Color.white);
        raceRegisterButton = Button("Race Register", actions.transform, "Add", .38f, .09f, .95f, .91f);
        raceRegisterButton.GetComponent<Image>().color = Navy;
        raceAction = raceRegisterButton.GetComponentInChildren<Text>();

        confirmation = Box("Add Registered Race", racePanel.transform, 0, 0, 1, 1, Color.white);
        var panel = confirmation.transform;
        Label("Add to Event List", panel, .08f, .77f, .92f, .9f, true);
        Label("Choose which RunSignup event to add to Erny.", panel, .08f, .65f, .92f, .77f);
        confirmationStatus = Label("", panel, .08f, .3f, .92f, .43f, false, Red);
        checkoutBackButton = Button("Back to checkout", panel, "Back", .08f, .19f, .34f, .28f);
        confirmRegistration = Button("Save race", panel, "Add to Event List", .38f, .19f, .92f, .28f);
        eventChoice = Drop("Race Event", panel, new[] { "Select your event" }, .08f, .55f, .92f, .64f);
        confirmation.SetActive(false);
        racePanel.SetActive(false);
        return racePanel;
    }

    private void LoadRaceAddress(Uri address)
    {
        browserAvailable = browser.Open(address.AbsoluteUri);
        browserStatus.text = browserAvailable ? browser.Status : "The embedded browser is unavailable on this platform.";
    }

    private void LateUpdate()
    {
        if (racePanel == null || !racePanel.activeInHierarchy) return;
        if (!confirmation.activeSelf)
        {
            browser.Layout(webArea, raceActions, raceActionsRestingPosition, webCorners);
            if (browserAvailable && browserStatus.text != browser.Status) browserStatus.text = browser.Status;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) RaceBack();
#endif
    }

    private void AddRace()
    {
        if (savingRace) return;
        if (raceAdded || entries.Any(entry => eventgetter.HasDiscoveredRace(selectedRace, entry)))
        {
            owner.screencurenter(1);
            return;
        }
        if (entries.Length == 1) SaveRace(entries[0]);
        else ShowEventPicker("");
    }

    private void ShowEventPicker(string message)
    {
        browser.SetVisible(false);
        confirmation.SetActive(true);
        confirmationStatus.text = message;
        eventChoice.gameObject.SetActive(entries.Length > 1);
        eventChoice.ClearOptions();
        var options = entries.Select(entry => entry.name + (entry.date == default ? ""
            : " (" + entry.date.ToString("M/d/yy", CultureInfo.InvariantCulture) + ")")).ToList();
        if (entries.Length > 1) options.Insert(0, "Select your event");
        eventChoice.AddOptions(options);
        eventChoice.SetValueWithoutNotify(0);
        eventChoice.RefreshShownValue();
    }

    private void SetRaceAdded(bool added)
    {
        raceAdded = added;
        bool showRunner = added && addedRaceIcon != null && raceRegisterIcon != null;
        if (raceRegisterIcon != null)
        {
            raceRegisterIcon.overrideSprite = showRunner ? addedRaceIcon : null;
            raceRegisterIcon.preserveAspect = showRunner || originalIconAspect;
        }
        if (raceAction != null) raceAction.text = added ? "Added" : "Add";
    }

    private RaceEntry ChosenEntry => entries.Length == 1 ? entries[0]
        : eventChoice.value == 0 ? null : entries[eventChoice.value - 1];

    private void SaveRace(RaceEntry entry)
    {
        if (savingRace || entry == null)
        {
            if (!savingRace) confirmationStatus.text = "Select the event you want to add.";
            return;
        }
        savingRace = true;
        confirmRegistration.interactable = false;
        confirmationStatus.text = "Saving race...";
        owner.SaveDiscoveredRace(selectedRace, entry, error => {
            savingRace = false;
            confirmRegistration.interactable = true;
            if (error == null) SetRaceAdded(true);
            else if (confirmation.activeSelf) confirmationStatus.text = error;
            else if (racePanel.activeInHierarchy) ShowEventPicker(error);
        });
    }

    private void RaceBack()
    {
        if (savingRace) return;
        if (confirmation.activeSelf)
        {
            confirmation.SetActive(false);
            browser.SetVisible(true);
        }
        else if (browser.IsOnSubpage(raceAddress))
        {
            LoadRaceAddress(raceAddress);
        }
        else CloseRacePage();
    }

    private void CloseRacePage()
    {
        browser.Dispose();
        if (racePanel == null) return;
        if (racePageBound) raceActions.anchoredPosition = raceActionsRestingPosition;
        confirmation.SetActive(false);
        racePanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        searching = false; resultsPanel.SetActive(true); filtersPanel.SetActive(false);
        status.text = message; status.gameObject.SetActive(true);
    }

    public void Clear()
    {
        raceName.text = homeSearch.text = resultsSearch.text = zip.text = minDistance.text = maxDistance.text = "";
        raceType.value = radius.value = country.value = fromMonth.value = toMonth.value = 0;
        state.value = 0;
        metric.isOn = false; Submit();
    }

    private void ClearRows()
    {
        foreach (var row in rows) { row.SetActive(false); Destroy(row); } rows.Clear();
        foreach (var asset in logoAssets) Destroy(asset); logoAssets.Clear();
    }
    private void EnsurePagination()
    {
        if (nextPage == null)
        {
            nextPage = Button("Next page", resultsPanel.transform, "Next >", .67f, .015f, .95f, .065f);
            previousPage = Button("Previous page", resultsPanel.transform, "< Previous", .05f, .015f, .33f, .065f);
            pageLabel = Label("Page 1", resultsPanel.transform, .37f, .015f, .63f, .065f);
            var scroll = (RectTransform)resultsContent.GetComponentInParent<ScrollRect>().transform;
            scroll.anchorMin = new Vector2(.035f, .085f);
        }
        nextPage.onClick.AddListener(() => ChangePage(1));
        previousPage.onClick.AddListener(() => ChangePage(-1));
        nextPage.gameObject.SetActive(false); previousPage.gameObject.SetActive(false); pageLabel.gameObject.SetActive(false);
    }
    private void EnsureStateDropdown()
    {
        if (state == null)
        {
            var old = filtersPanel.transform.Find("State Province");
            if (old != null) { old.name = "Legacy State Input"; old.gameObject.SetActive(false); }
            state = Drop("State Province", filtersPanel.transform, new[] { "State/Prov." }, .58f, .435f, .925f, .525f);
        }
        if (state.options.Count < 2) RefreshStates();
    }
    private void RefreshStates()
    {
        string codes = country.value == 0
            ? "AL AK AZ AR CA CO CT DE DC FL GA HI ID IL IN IA KS KY LA ME MD MA MI MN MS MO MT NE NV NH NJ NM NY NC ND OH OK OR PA RI SC SD TN TX UT VT VA WA WV WI WY"
            : country.value == 1 ? "AB BC MB NB NL NS NT NU ON PE QC SK YT"
            : "AGU BCN BCS CAM CHP CHH COA COL DUR GUA GRO HID JAL MEX MIC MOR NAY NLE OAX PUE QUE ROO SLP SIN SON TAB TAM TLA VER YUC ZAC CMX";
        state.ClearOptions(); state.AddOptions(new[] { "State/Prov." }.Concat(codes.Split(' ')).ToList()); state.value = 0;
    }
    private void ChangePage(int delta)
    {
        if (searching || page + delta < 1) return;
        CancelSearch(); page += delta; ClearRows(); searching = true;
        status.text = "Searching races..."; status.gameObject.SetActive(true);
        StartCoroutine(SearchRunSignup());
    }
    private IEnumerator SearchRunSignup()
    {
        List<string> urls;
        try { urls = RunSignupRaceSearch.BuildUrls(CurrentFilters, page, DateTime.Today); }
        catch (ArgumentException e) { ShowMessage(e.Message); yield break; }
        var found = new Dictionary<string, RaceResult>(); bool hasMore = false;
        foreach (string url in urls)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                activeRequest = request; request.timeout = 20;
                yield return request.SendWebRequest(); activeRequest = null;
                if (request.result != UnityWebRequest.Result.Success)
                { ShowMessage("Race search failed. Check your connection and try again. (" + request.responseCode + ")"); yield break; }
                List<RaceResult> batch;
                try { batch = RunSignupRaceSearch.Parse(request.downloadHandler.text); }
                catch (Exception) { ShowMessage("RunSignup returned an unexpected response. Please try again."); yield break; }
                hasMore |= batch.Count >= RunSignupRaceSearch.PageSize;
                foreach (var race in batch) if (!string.IsNullOrWhiteSpace(race.id)) found[race.id] = race;
            }
        }
        SetResults(found.Values.OrderBy(r => r.sortDate).ThenBy(r => r.name));
        pageLabel.text = "Page " + page;
        nextPage.gameObject.SetActive(hasMore); previousPage.gameObject.SetActive(page > 1); pageLabel.gameObject.SetActive(true);
        // Download one logo at a time rather than spiking memory with parallel image decodes.
        for (int i = 0; i < races.Count; i++)
        {
            if (!Uri.TryCreate(races[i].logoUrl, UriKind.Absolute, out Uri logoUri) || logoUri.Scheme != "https") continue;
            using (var request = UnityWebRequestTexture.GetTexture(logoUri.AbsoluteUri, true))
            {
                activeRequest = request; request.timeout = 10;
                yield return request.SendWebRequest(); activeRequest = null;
                if (request.result != UnityWebRequest.Result.Success) continue;
                var texture = DownloadHandlerTexture.GetContent(request);
                var sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
                logoAssets.Add(texture); logoAssets.Add(sprite);
                var image = rows[i].transform.Find("Logo").GetComponent<Image>(); image.sprite = sprite; image.enabled = true;
            }
        }
    }
    private void CancelSearch()
    {
        var request = activeRequest; activeRequest = null;
        if (request != null) request.Abort();
        StopAllCoroutines();
        searching = false;
    }
    private void OnDisable() { CloseRacePage(); CancelSearch(); }
    private void OnDestroy() { CancelSearch(); foreach (var asset in logoAssets) Destroy(asset); if (Instance == this) Instance = null; }
    private static bool ValidDistance(string value) => string.IsNullOrWhiteSpace(value)
        || (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float n) && n >= 0 && !float.IsInfinity(n));

#if UNITY_EDITOR
    public void PreviewResults()
    {
        resultsPanel.SetActive(true); filtersPanel.SetActive(false);
        SetResults(new[] {
            new RaceResult { name = "The SKIPPO Trail Race", date = "Sat 10/31/26", location = "Ballwin, MO", distances = "10K    13.1 Miles    30K" },
            new RaceResult { name = "Clarksburg Country Run", date = "Sun 11/1/26", location = "Clarksburg, CA", distances = "18.6 Miles    13.1 Miles    6.2 Miles    3.1 Miles" }
        });
        status.text = "Design preview only - not live race results"; status.gameObject.SetActive(true);
    }
#endif

    private Text Label(string text, Transform parent, float x, float y, float x2, float y2, bool heading = false, Color? color = null)
    {
        var t = Box(text, parent, x, y, x2, y2, null).AddComponent<Text>();
        t.text = text; t.font = heading ? headingFont : bodyFont; t.color = color ?? Navy;
        t.supportRichText = false;
        t.fontSize = heading ? 90 : 65; t.resizeTextForBestFit = true; t.resizeTextMinSize = 26; t.resizeTextMaxSize = t.fontSize;
        t.alignment = heading ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft; t.raycastTarget = false;
        return t;
    }
    private Button Button(string name, Transform parent, string text, float x, float y, float x2, float y2)
    {
        var go = Box(name, parent, x, y, x2, y2, Red); var b = go.AddComponent<Button>();
        var label = Label(text, go.transform, .03f, .02f, .97f, .98f, false, Color.white); label.alignment = TextAnchor.MiddleCenter;
        return b;
    }
    private InputField Input(string name, Transform parent, string hint, float x, float y, float x2, float y2)
    {
        var go = Box(name, parent, x, y, x2, y2, Color.white); var image = go.GetComponent<Image>();
        image.sprite = fieldSprite; image.type = Image.Type.Sliced;
        if (fieldSprite == null) go.AddComponent<Outline>().effectColor = Navy;
        var input = go.AddComponent<InputField>(); input.targetGraphic = image;
        input.textComponent = Label("", go.transform, .04f, .04f, .96f, .96f);
        input.placeholder = Label(hint, go.transform, .04f, .04f, .96f, .96f, false, Green);
        input.lineType = InputField.LineType.SingleLine; input.characterLimit = 120;
        return input;
    }
    private Dropdown Drop(string name, Transform parent, string[] options, float x, float y, float x2, float y2)
    {
        var go = Box(name, parent, x, y, x2, y2, Color.white);
        go.GetComponent<Image>().sprite = fieldSprite; go.GetComponent<Image>().type = Image.Type.Sliced;
        var drop = go.AddComponent<Dropdown>(); drop.targetGraphic = go.GetComponent<Image>();
        drop.captionText = Label(options[0], go.transform, .04f, .04f, .87f, .96f);
        Label("v", go.transform, .9f, .05f, .98f, .95f);
        drop.AddOptions(options.ToList());
        var template = Box("Template", go.transform, 0, 0, 1, 0, Color.white);
        var tr = (RectTransform)template.transform; tr.pivot = new Vector2(.5f, 1); tr.sizeDelta = new Vector2(0, 500);
        var viewport = Box("Viewport", template.transform, 0, 0, 1, 1, Color.white);
        viewport.AddComponent<RectMask2D>();
        var content = Box("Content", viewport.transform, 0, 1, 1, 1, null);
        var cr = (RectTransform)content.transform; cr.pivot = new Vector2(.5f, 1); cr.sizeDelta = new Vector2(0, 120);
        var item = Box("Item", content.transform, 0, .0f, 1, 1, Color.white);
        var toggle = item.AddComponent<Toggle>(); toggle.targetGraphic = item.GetComponent<Image>();
        toggle.graphic = Box("Selected", item.transform, .015f, .2f, .04f, .8f, Green).GetComponent<Image>();
        drop.itemText = Label("Option", item.transform, .07f, .02f, .98f, .98f);
        var scroll = template.AddComponent<ScrollRect>(); scroll.content = cr; scroll.viewport = (RectTransform)viewport.transform;
        scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
        drop.template = tr; template.SetActive(false); drop.RefreshShownValue(); return drop;
    }
    private static GameObject Box(string name, Transform parent, float x, float y, float x2, float y2, Color? color)
    {
        var go = new GameObject(name, typeof(RectTransform)); go.layer = 5; go.transform.SetParent(parent, false);
        Place((RectTransform)go.transform, x, y, x2, y2);
        if (color.HasValue) { var image = go.AddComponent<Image>(); image.color = color.Value; }
        return go;
    }
    private static void Place(RectTransform rect, float x, float y, float x2, float y2)
    {
        rect.anchorMin = new Vector2(x, y); rect.anchorMax = new Vector2(x2, y2);
        rect.offsetMin = rect.offsetMax = Vector2.zero; rect.localScale = Vector3.one;
    }
}
