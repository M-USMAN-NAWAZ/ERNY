using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfoff : MonoBehaviour
{
    public GameObject object1;
    public static int counter;
	private void Awake()
	{
        if (counter == 0)
        {
            object1.SetActive(true);
        }
    
    }
	// Start is called before the first frame update
	void Start()
    {
        
        
        
        StartCoroutine(turnoff());
     
   
    
    
    }




    IEnumerator turnoff()
    {
        yield return new WaitForSeconds(2);
        counter += 1;
               object1.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
