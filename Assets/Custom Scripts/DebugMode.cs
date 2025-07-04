using UnityEngine;

public class DebugMode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject body = GameObject.Find("RPM_Photon_Test_Character");
        if (body != null)
        body.SetActive(false);
    }
}
