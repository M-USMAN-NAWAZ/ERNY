using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DetailsPannelManager : MonoBehaviour
{
	public Text title;
	public Text subtitletitle;
	public Text details;




	public void Setcontent(string a , string b,string c) 
	{
		title.text = a;
		subtitletitle.text = b;
		details.text = c;

	}
}
