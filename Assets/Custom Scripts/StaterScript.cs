using UnityEngine;

public class StaterScript : MonoBehaviour
{
    public PhotonManager photonManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photonManager.playerControlCanvas.CanvasEnable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
