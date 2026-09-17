using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class guestevent : MonoBehaviour
{
	public static int check;
	public Button b1, b2, b3;

	public GameObject popup;
	public ScrollRect subscribe;
	public GameObject[] toshowdata;

	
	public void resetscroll()
	{
		subscribe.verticalNormalizedPosition = 1f;
	}
	public void toshow()
	{
		for (int i = 0; i < toshowdata.Length; i++)
		{
			toshowdata[i].SetActive(true);




		}
	}
	private void Awake()
	{

		if (check == 0)
		{

			for (int i = 0; i < toshowdata.Length; i++)
			{
				toshowdata[i].SetActive(false);




			}
		}
		else
		{


			for (int i = 0; i < toshowdata.Length; i++)
			{
				toshowdata[i].SetActive(true);
			}
		}
		check = 1;











		if (ApiRequestGenerator.geustpopup == 1)
		{
			ApiRequestGenerator.geustpopup = 0;
		//	popup.SetActive(true);
		///	guestpopup.instance.pp.Play("popp");
		//	pops.instance.header.text = "Uh oh!";
			//pops.instance.description.text = "Your Event Name, Date and Location are required to advance on the course.";


		}
	}
	private void Update()
	{/*
		b1.interactable = false;
		b2.interactable = false;
		b3.interactable = false;
*/	}


}
