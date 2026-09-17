using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletegoal : MonoBehaviour
{
    public GameObject goal;

    public void destroyobject()
    {
        Destroy(goal);
    }


    public void eventdelete()
    {
        eventgetter.instance.goalminuser();
        Destroy(goal);
    }

    public void updatedelete()
    {
        updataeven.instance.goalminuser();
        Destroy(goal);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
