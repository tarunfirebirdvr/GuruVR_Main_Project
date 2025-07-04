using StarterAssets;
using System.Collections;
using UnityEngine;

public class playerControlCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject playerControlCanvas;
    [SerializeField] ThirdPersonController tpc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControlCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanvasEnable()
    {
        playerControlCanvas.SetActive(true);
        StartCoroutine(DelayedAction());
    }
       IEnumerator DelayedAction()
        {
            Debug.Log("Action will happen in 2 seconds...");
            yield return new WaitForSeconds(3f);
       tpc.Gravity = -10;
    }
    public void CanvasDisable()
    {
        playerControlCanvas.SetActive(false);
    }
}
