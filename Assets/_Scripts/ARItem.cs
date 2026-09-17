using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARItem : MonoBehaviour
{
	public string title;
	public string subtitle;
	public string details;


	public DetailsPannelManager instance;
	public void setDetails() 
	{
		instance.Setcontent(title, subtitle, details);


	}


}
