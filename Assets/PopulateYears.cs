using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
public class PopulateYears : MonoBehaviour
{
    public Dropdown dp;
      public List<string> asd= new List<string>();
    public  bool eventcheck;

    public int noToAddInYear;
  private void Awake()
    {
        //  dp.ClearOptions();
        if (eventcheck)
        {
            if (System.DateTime.Now.Year != 2024)
            {
                Debug.Log("current year: " + System.DateTime.Now.Year);
              //  asd.Add("2025");
              //  asd.Add("2024");




            }
			if (System.DateTime.Now.Year != 2025)
			{
				Debug.Log("current year: " + System.DateTime.Now.Year);

			//	asd.Add("2026");
			//	asd.Add("2025");



			}
			else
			{
				asd.Add("2026");

			}


            for (int i = System.DateTime.Now.Year + noToAddInYear; i >= System.DateTime.Now.Year - (92 + noToAddInYear); i--)
            {
                asd.Add(i.ToString());
            }
            dp.AddOptions(asd);

        }
        else
        {
            for (int i = System.DateTime.Now.Year; i >= System.DateTime.Now.Year - 92; i--)
            {
                asd.Add(i.ToString());
            }
            dp.AddOptions(asd);
        }

    }


}
