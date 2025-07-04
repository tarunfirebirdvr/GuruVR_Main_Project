using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class StopDelay : MonoBehaviour
{
    public UnityEvent onDelayedEvent;
    public float delayInSeconds = 2f;

    void Start()
    {
        StartCoroutine(CallEventAfterDelay());
    }

    IEnumerator CallEventAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        onDelayedEvent?.Invoke();
    }
}
