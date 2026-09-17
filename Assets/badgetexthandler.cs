using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VoxelBusters.ReplayKit.Common.Utility.ThirdParty.ExifLibrary;

public class badgetexthandler : MonoBehaviour
{

    public string header, explain;

    public bool normalbadge,specialbadge;

    public string eventname;

    [Header("Image Download")]

    public bool isCategory1;
    public string URL;
    public string id;
    public string heading;
    public Image badgeImage;

    public Sprite spr;

    

    void Start()
    {
        Debug.Log("Inside the badgetexthandler Start");
        StartCoroutine(WaitForPeremeters());
    }


    IEnumerator WaitForPeremeters()
    {
        if(URL == "" || id == null|| badgeImage == null)
        {

            yield return null;
        }
        BadgeImageManager.instance.ApplyBadgeImage(id, URL, badgeImage, heading, spr);
    }

    public void showtext()
    {
        Debug.Log("Show Text");
        pops.instance.OnButton();
        pops.instance.showImg(badgeImage.sprite);
        pops.instance.pp.Play("popp");
        if (normalbadge)
        {
            pops.instance.header.text = header;
            pops.instance.description.text = explain;
		}
		else
		{
			pops.instance.description.resizeTextMaxSize = 100;
			string temp= "Congrats on crushing the \r\n" + eventname;
            if (heading == "")
            {
                pops.instance.header.text = "YOU DID IT!";
            }
            else
            {
                pops.instance.header.text = heading;
            }
            pops.instance.description.text = temp;


		}

		pops.instance.popbox.SetActive(true);

        eventgetter.instance.UpgradeBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
