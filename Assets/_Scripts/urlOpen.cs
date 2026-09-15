using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.ReplayKit.Common.Utility;

public class urlOpen : MonoBehaviour
{
	public string adurl3, adurl2, adUrl;

	public void HelpUrl()
	{
		Application.OpenURL("https://blingar.co/faqs");
    }

	public void openurl() 
	{
		if (adimage.currentadrun == 2)
		{
			Application.OpenURL(adurl3);
		}
		else if (adimage.currentadrun == 1)
		{

			Application.OpenURL(adurl2);

		}
		else if (adimage.currentadrun == 0)
		{
			Application.OpenURL(adUrl);


		}
	}
}
