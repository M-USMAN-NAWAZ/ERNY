using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandOnClick : MonoBehaviour
{
    public static RectTransform LastExpandedWindow { get; private set; }

    public ScrollRect scrool;
    public RectTransform ExpandedWindow;
    public Image ExpandedIndicator;
    public Sprite expanded;
    public Sprite shrunk;
    public bool clicked;
    public float duration;
    public int click;
   public Vector2 myexpandsize;
    public ScrollRect parentscroll;
    public GameObject[] collabbuttons;
    public int goalchec;
    private float initialExpandedHeight;
    private bool expandedSizeApplied;

    private void Start()
    {
        goalchec = 0;
        click = 0;
        DOTween.Init();
        clicked = false;

        myexpandsize = ExpandedWindow.sizeDelta;
        initialExpandedHeight = myexpandsize.y;
    }

    private void Update()
    {
        if (!clicked || ExpandedWindow == null ||
            ExpandedWindow.name != "GalleryOpen" ||
            !ExpandedWindow.gameObject.activeInHierarchy)
            return;

        Vector2 position;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;
            position = touch.position;
        }
        else if (Input.GetMouseButtonDown(0))
            position = Input.mousePosition;
        else
            return;

        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
            return;

        var hits = new List<RaycastResult>();
        eventSystem.RaycastAll(
            new PointerEventData(eventSystem) { position = position }, hits);
        if (hits.Count == 0)
            return;

        Button button = hits[0].gameObject.GetComponentInParent<Button>();
        if (button == null || !button.IsInteractable() ||
            button.transform == transform ||
            button.transform.IsChildOf(ExpandedWindow) ||
            button.GetComponentInParent<GalleryPreviewPanel>() != null ||
            button.GetComponentInParent<ImageCropper>() != null)
            return;

        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            if (button.onClick.GetPersistentTarget(i) is EventGalleryPicker &&
                (button.onClick.GetPersistentMethodName(i) == "PickFromGallery" ||
                 button.onClick.GetPersistentMethodName(i) == "CaptureFromCamera"))
                return;
        }

        Shrink();
    }




    //adcard goal area


    public void addgoal()
    {
        goalchec += 1;


    }

    public void addcardOnclick()
    {
        clicked = !clicked;
        if (clicked)
        {
            click = 1;
            myaddgoalExpand();
        }
        else if (!clicked)
        {
            mygoalshrunkaddcard();
        }
    }

    public void myaddgoalExpand()
    {
        Debug.Log("Expand Me");
        // ExpandedIndicator.sprite = expanded;
        click = 1;
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + eventgetter.instance.sizeofgoal.y * goalchec);
        scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y + myexpandsize.y), duration, false);
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y+ myexpandsize.y);
        ExpandedWindow.gameObject.transform.DOScaleY(1, duration);
        ExpandedWindow.gameObject.SetActive(true);
    }
    public void neweventmygoalshrunkaddcard()
    {if(clicked)
        {
            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y - eventgetter.instance.sizeofgoal.y * goalchec);
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
        }
        IEnumerator turnoffobjet(GameObject x)
        {
            yield return new WaitForSeconds(duration / 2);
            x.SetActive(false);
        }
    }
    public void mygoalshrunkaddcard()
    {
        {

            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y  - eventgetter.instance.sizeofgoal.y * goalchec);
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
        }
        IEnumerator turnoffobjet(GameObject x)
        {
            yield return new WaitForSeconds(duration / 2);
            x.SetActive(false);
        }
    }
    ////////////////////////////////////////////////////event card data
    public void eventcardcardOnclick()
    {
        clicked = !clicked;
        if (clicked)
        {
            click = 1;
            eventcardgoalExpand();
        }
        else if (!clicked)
        {
            eventcaardshrunkcard();
        }
    }

    public void eventcardgoalExpand()
    {
        Debug.Log("Expand Me");
        // ExpandedIndicator.sprite = expanded;
        click = 1;
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + 100 * recievedata.goalcounterforexpand);
        scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y + myexpandsize.y), duration, false);
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y+ myexpandsize.y);
       // ExpandedWindow.gameObject.transform.DOScaleY(1, duration);
        ExpandedWindow.gameObject.SetActive(true);
    }

    public void eventcaardshrunkcard()
    {
        {
            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y - 100 * recievedata.goalcounterforexpand);
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.SetActive(false);
         //   ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            //StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
        }
        IEnumerator turnoffobjet(GameObject x)
        {
            yield return new WaitForSeconds(duration / 2);
            x.SetActive(false);
        }
    }


   ////////////////////////////////////////////////////////update card data

    public void updatecardcardOnclick()
    {
        clicked = !clicked;
        if (clicked)
        {
            click = 1;
            updatecardgoalExpand();
        }
        else if (!clicked)
        {
            updatecardshrunkcard();
        }
    }

    public void updatecardgoalExpand()
    {
        Debug.Log("Expand Me");
        // ExpandedIndicator.sprite = expanded;
        click = 1;
        parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y + updataeven.instance.sizeofgoal.y * updataeven.instance.countofgoals);
        scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y + myexpandsize.y), duration, false);
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y+ myexpandsize.y);
        ExpandedWindow.gameObject.transform.DOScaleY(1, duration);
        ExpandedWindow.gameObject.SetActive(true);
    }

    public void updatecardshrunkcard()
    {
        {
            parentscroll.content.sizeDelta = new Vector2(parentscroll.content.sizeDelta.x, parentscroll.content.sizeDelta.y - updataeven.instance.sizeofgoal.y * updataeven.instance.countofgoals);
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
        }
        IEnumerator turnoffobjet(GameObject x)
        {
            yield return new WaitForSeconds(duration / 2);
            x.SetActive(false);
        }
    }









    public static void RefreshGalleryHeight(RectTransform gallery)
    {
        if (gallery == null)
            return;

        GridLayoutGroup grid = gallery.GetComponent<GridLayoutGroup>();
        if (grid == null)
            return;

        int itemCount = 0;
        foreach (Transform child in gallery)
        {
            LayoutElement childLayout = child.GetComponent<LayoutElement>();
            if (child.gameObject.activeSelf &&
                (childLayout == null || !childLayout.ignoreLayout))
                itemCount++;
        }

        int columns = grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount
            ? Mathf.Max(1, grid.constraintCount)
            : Mathf.Max(1, Mathf.FloorToInt(
                (gallery.rect.width - grid.padding.horizontal + grid.spacing.x) /
                (grid.cellSize.x + grid.spacing.x)));
        int rows = itemCount == 0
            ? 0
            : Mathf.CeilToInt((float)itemCount / columns);
        float contentHeight = grid.padding.vertical;

        if (rows > 0)
            contentHeight += rows * grid.cellSize.y +
                (rows - 1) * grid.spacing.y;

        ExpandOnClick[] expanders = FindObjectsByType<ExpandOnClick>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (ExpandOnClick expander in expanders)
        {
            if (expander.ExpandedWindow == gallery)
            {
                expander.ApplyExpandedHeight(contentHeight);
                return;
            }
        }
    }

    private void ApplyExpandedHeight(float contentHeight)
    {
        if (ExpandedWindow == null)
            return;

        if (initialExpandedHeight <= 0f)
            initialExpandedHeight = Mathf.Max(
                myexpandsize.y,
                ExpandedWindow.rect.height);

        float targetHeight = Mathf.Max(initialExpandedHeight, contentHeight);
        float heightChange = targetHeight - myexpandsize.y;
        myexpandsize.y = targetHeight;

        LayoutElement layout = ExpandedWindow.GetComponent<LayoutElement>();
        if (layout != null)
            layout.preferredHeight = targetHeight;

        ExpandedWindow.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetHeight);

        if (expandedSizeApplied && scrool != null && scrool.content != null &&
            !Mathf.Approximately(heightChange, 0f))
        {
            scrool.content.sizeDelta = new Vector2(
                scrool.content.sizeDelta.x,
                scrool.content.sizeDelta.y + heightChange);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ExpandedWindow);
        if (scrool != null && scrool.content != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrool.content);
    }

    public void Onclick() 
    {
        clicked = !clicked;
        if (clicked)
        {
            click = 1;
            Expand();
        }
        else if (!clicked) 
        {
            Shrink();
        }    
    }

 
  
    public void Expand() 
    {
        LastExpandedWindow = ExpandedWindow;
        EventGalleryPicker.Instance?.BindImageRow(ExpandedWindow);

        Debug.Log("Expand Me");
        // ExpandedIndicator.sprite = expanded;
        click = 1;

        scrool.content.GetComponent<VerticalLayoutGroup>().enabled = false;
        scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y + myexpandsize.y), 0f, true);
        expandedSizeApplied = true;
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y+ myexpandsize.y);
        ExpandedWindow.gameObject.transform.DOScaleY(1, 0f);
        ExpandedWindow.gameObject.SetActive(true);

        scrool.content.GetComponent<VerticalLayoutGroup>().enabled = true;
    }


    public void checkingopenGOAL()
    {
        if (click == 1)
        {
            Shrunk();
        }
        else
        {
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            // scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
         //   ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            ExpandedWindow.gameObject.SetActive(false);

        }
    }

    public void  checkingopen()
    {
        if(click==1)
        {
            Shrunk();
        }else
        {
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            // scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));

        }
    }




    public void Shrunk()
    {
        clicked = false;
        Debug.Log("Shrink Me");
        //  ExpandedIndicator.sprite = shrunk;
        ///   scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
        ExpandedWindow.gameObject.SetActive(false);
        //StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
    }
    public void newShrink()
    {
        if (clicked)
        {
            clicked = false;
            Debug.Log("Shrink Me");
            //  ExpandedIndicator.sprite = shrunk;
            scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), duration, false);
            //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
            ExpandedWindow.gameObject.transform.DOScaleY(0, duration);
            StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
        }
    }


    public void Shrink() 
    {
        clicked = false;
        if (LastExpandedWindow == ExpandedWindow)
            LastExpandedWindow = null;
        Debug.Log("Shrink Me");
      //  ExpandedIndicator.sprite = shrunk;
       scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), 0f, true);
        expandedSizeApplied = false;
        //        scrool.content.sizeDelta = new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y);
        ExpandedWindow.gameObject.transform.DOScaleY(0, 0);
        StartCoroutine(turnoffobjet(ExpandedWindow.gameObject));
    }
    IEnumerator turnoffobjet(GameObject x) 
    {
        yield return new WaitForSeconds(duration/2);
        x.SetActive( false);
    }
}
