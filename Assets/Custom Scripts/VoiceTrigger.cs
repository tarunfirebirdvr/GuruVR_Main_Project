using UnityEngine;
using UnityEngine.Events;

public class VoiceTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public bool hasTriggered = false;
    public AudioSource voiceSource;
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            voiceSource.Stop();
            hasTriggered = true;
            onTriggerEnterEvent.Invoke();
        }
    }
}
