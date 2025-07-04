using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GateController : MonoBehaviour
{
    public TurnActiveScript leverA;
    public TurnActiveScript leverB;
    public GameObject gateToOpen;
    public UnityEvent Uis;
    public AudioSource AudioSource;
    public void CheckLevers()
    {
        if (leverA.hasTriggered && leverB.hasTriggered)
        {
            
            StartCoroutine(CallEventAfterDelay());

           // gateToOpen.SetActive(false); // or .SetActive(true) if you want to show the open gate
            Debug.Log("Gate opened!");
        }
    }
    IEnumerator CallEventAfterDelay()
    {
        yield return new WaitForSeconds(0);
        gateToOpen.SetActive(true);
        AudioSource.Stop();
        Uis.Invoke();
    }
}
