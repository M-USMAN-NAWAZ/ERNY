using NatSuite.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class microphoenmanager : MonoBehaviour
{

    public arcameracapture arcam;

    // Start is called before the first frame update
    void Start()
    {
       // stoprecorder();


	}


    public void playrecorder()
    {
        arcam.startmicrophone();
    
    }


	public void stoprecorder()
	{
        
        
        arcam.stopmicrophone();

	}



	// Update is called once per frame
	void Update()
    {
        
    }
}
