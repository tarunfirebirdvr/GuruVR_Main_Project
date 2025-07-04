using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidercheckforEMIOn : MonoBehaviour
{

    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject Disablearrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Arrow.SetActive(true);
            UI.SetActive(true);
            Disablearrow.SetActive(false);
        }
    }
}
