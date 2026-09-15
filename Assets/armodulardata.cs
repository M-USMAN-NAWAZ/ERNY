using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MText;
public class armodulardata : MonoBehaviour
{
    public GameObject close, away;

    public TextMeshProUGUI name, distance, time, unit, unit2;
    //        public Modular3DText name, distance, time,unit,unit2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(work());

        StartCoroutine(death());
    }
    public void vuforiaar()
    {

      StartCoroutine(work());

      StartCoroutine(death());

        Debug.Log("work: "+ardata.mtime);
    }

    IEnumerator work()
        {
            yield return new WaitForSeconds(2);
            name.text = ardata.ename;
            distance.text = ardata.edistance;
            time.text = ardata.etime;
            unit.text = ardata.edistenaceunit;
            unit2.text = ardata.edistenaceunit;
        Debug.Log("data: "+ardata.edistance + " "+ardata.mdistance);
        }


        IEnumerator death()
        {
            yield return new WaitForSeconds(5);
            if (ardata.checks == 1)
            {
                close.SetActive(true);
                away.SetActive(false);
            }
            else
            {
                 close.SetActive(false);
                 away.SetActive(true);
            }
        }


        // Update is called once per frame
        void Update()
        {
      name.text = ardata.ename;
        distance.text = ardata.edistance;
        time.text = ardata.etime;
        unit.text = ardata.edistenaceunit;
        unit2.text = ardata.edistenaceunit;


        if (ardata.checks == 1)
        {
            close.SetActive(true);
            away.SetActive(false);
        }
        else
        {
            close.SetActive(false);
            away.SetActive(true);
        }
    }
    }
