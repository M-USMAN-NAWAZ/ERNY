using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evntcollabbbutton : MonoBehaviour
{
    public GameObject[] buttons, panels;
    public int check = 0;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void buttonno(int num)
    {
        check = num;
        StartCoroutine(panelscheck());
    }

    IEnumerator panelscheck()
    {
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < panels.Length; i++)
        {
            if (i != check)
            {
                if (panels[i].activeInHierarchy)
                {

                    if (i == 0)
                    {
                        buttons[i].GetComponent<ExpandOnClick>().eventcaardshrunkcard();

                    }
                    else
                    {
                        buttons[i].GetComponent<ExpandOnClick>().Onclick();
                    }
                }


            }


        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
