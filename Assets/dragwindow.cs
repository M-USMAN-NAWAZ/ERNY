using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class dragwindow : MonoBehaviour,IDragHandler
{
    
    
        public Vector2 xValues;
        public RectTransform dragable;
    public RectTransform check;
    public bool dragablee;
        public void OnDrag(PointerEventData eventData)
        {




        if (dragablee)
      {
            dragable.anchoredPosition += eventData.delta;
      dragable.anchoredPosition += new Vector2(709, eventData.delta.y );
      }
        
     if (dragable.anchoredPosition.x<xValues.x)
           {
   //         dragablee = false;

           }
    /*       else if (check.anchoredPosition.x < xValues.x)
           {

            dragablee = false;
        
            }


        */


    }
    



    // Start is called before the first frame update
    void Start()
        {
       // check.transform.position = dragable.transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }















}
