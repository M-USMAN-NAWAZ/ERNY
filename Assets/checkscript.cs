using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MText
{
    public class checkscript : MonoBehaviour
    {
        Modular3DText Text => gameObject.GetComponent<Modular3DText>();

        // Start is called before the first frame update
        void Start()
        {
            Text.Text = "yahpooo;";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
