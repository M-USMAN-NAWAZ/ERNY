using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandOnClick : MonoBehaviour
{
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
    private void Start()
    {
        goalchec = 0;
        click = 0;
        DOTween.Init();
        clicked = false;

        myexpandsize = ExpandedWindow.sizeDelta;
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


        Debug.Log("Expand Me");
        // ExpandedIndicator.sprite = expanded;
        click = 1;

        scrool.content.GetComponent<VerticalLayoutGroup>().enabled = false;
        scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y + myexpandsize.y), 0f, true);
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
        Debug.Log("Shrink Me");
      //  ExpandedIndicator.sprite = shrunk;
       scrool.content.DOSizeDelta(new Vector2(scrool.content.sizeDelta.x, scrool.content.sizeDelta.y - myexpandsize.y), 0f, true);
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
