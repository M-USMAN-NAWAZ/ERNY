using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.UI;
public class CountryBasedJsonParser : MonoBehaviour
{
    public static CountryBasedJsonParser instance;
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [System.Serializable]
    public class Root
    {
        public string name;
        public string code;
    }


    public TextAsset jsonfile;

    public  Dropdown dropdown;
   
    [SerializeField]
    public Sprite[] flags;



	 void Start()
	{
        Debug.Log(jsonfile.text);

        
      

        List<Root> allcountries =  JsonConvert.DeserializeObject<List<Root>>(jsonfile.text.ToString());
       // dropdown.options.Clear();
        foreach (Root t in allcountries)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = t.name, image = flags.Where(obj => obj.name == t.code.ToLower()).SingleOrDefault()   });
        }

        dropdown.value = apigetter.countryid;
       // dropdown.value= 229;//default to us
        Debug.Log("balue "+ apigetter.countryid);
    }
    private void Update()
    {
       // Debug.Log("balue " + apigetter.countryid);
    }

}
