using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBasedcollider : MonoBehaviour
{
    public GameObject beam;
    public GameObject object1; // Assign GameObject named "1" in Inspector
    public GameObject object0; // Assign GameObject named "0" in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "1")
        {
            object1.SetActive(true);
            object0.SetActive(false);
        }
        else
        {
            object1.SetActive(false);
            object0.SetActive(true);
        }
        beam.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        beam.SetActive(true);
        object1.SetActive(false);
        object0.SetActive(false);
    }
}
