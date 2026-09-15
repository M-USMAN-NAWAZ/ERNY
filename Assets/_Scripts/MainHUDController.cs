	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHUDController : MonoBehaviour
{
	public Animator anim;
	public static int check;
	public void Transition_To_Screen(string x)		
	{
		anim.SetBool(x, true);

	}
	public void Transition_From_Screen(string x)
	{
		anim.SetBool(x, false);
	}
    
	
	private void Start()
    {
		if (check != 0)
		{
			Transition_From_Screen("Welcome");
			Transition_To_Screen("FaceFilters");
		}


    }
}
