using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameBasedtrigger : MonoBehaviour
{
    public TextMeshProUGUI txt;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "0")
        {
            
            txt.text = "0";
        }
        else
        {
            txt.text = "1";
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        txt.text = "";
    }
}
