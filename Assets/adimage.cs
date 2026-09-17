using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class adimage : MonoBehaviour
{

	public Image mainimage;
	public Sprite image1, image2, image3, image4, image5, image6, image7;
	public string adUrl, adurl2, adurl3;
	public bool check;
	public int ad3index;

	public static int currentadrun;
	// Start is called before the first frame update
	void Start()
	{


		InvokeRepeating("adchange", 2, 2);




	}




	public void adchange()
	{
		check = !check;

		if (currentadrun == 0)
		{
			//Debug.Log("checking: " + check);
			if (check)
			{

				mainimage.sprite = image1;

			}
			if (!check)
			{

				mainimage.sprite = image2;


			}
		}
		if (currentadrun == 1)
		{
			//Debug.Log("checking: " + check);
			if (check)
			{

				mainimage.sprite = image3;

			}
			if (!check)
			{

				mainimage.sprite = image4;


			}
		}

		if (currentadrun == 2)
		{
			ad3index += 1;
			//Debug.Log("checking: " + check);
			if (check)
			{

				mainimage.sprite = image5;

			}
			if (!check)
			{
				if (ad3index % 2 == 0)
				{
					mainimage.sprite = image6;
				}
				else
				{
					mainimage.sprite = image7;
				}

			}
		}










	}


	public void changead()
	{
		currentadrun += 1;
		if (currentadrun > 2)
		{

			currentadrun = 0;
		}




	}


	// Update is called once per frame
	public void OnAdClick()
	{
		if (currentadrun == 2)
		{
			Application.OpenURL(adurl3);
		}
		else if (currentadrun == 1)
		{

			Application.OpenURL(adurl2);

		}
		else if (currentadrun == 0)
		{
			Application.OpenURL(adUrl);


		}



	}
}
