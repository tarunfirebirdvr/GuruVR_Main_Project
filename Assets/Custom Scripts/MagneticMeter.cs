using UnityEngine;
using UnityEngine.Events;

public class MagneticMeter : MonoBehaviour
{
    public BoxCollider boxA;
    public BoxCollider boxB;
    public Transform needle; // Assign your needle GameObject here

    public float minYAngle = 0f;    // Angle when 0% inside (idle, center)
    public float maxYAngle = 90f;  // Idle (0%) rotation
    public int flag = 0;
    [SerializeField] private UnityEvent onTrigger;
    void Update()
    {
        Bounds boundsA = boxA.bounds;
        Bounds boundsB = boxB.bounds;

        // Calculate intersection on all 3 axes
        float xOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.x, boundsB.max.x) - Mathf.Max(boundsA.min.x, boundsB.min.x));
        float yOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.y, boundsB.max.y) - Mathf.Max(boundsA.min.y, boundsB.min.y));
        float zOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.z, boundsB.max.z) - Mathf.Max(boundsA.min.z, boundsB.min.z));

        float intersectionVolume = xOverlap * yOverlap * zOverlap;
        float volumeA = boundsA.size.x * boundsA.size.y * boundsA.size.z;

        float percentInside = (intersectionVolume / volumeA) * 100f;
        percentInside = Mathf.Clamp01(percentInside / 100f); // Normalize to 0–1

        // Rotate needle only on Y-axis
        float yAngle = Mathf.Lerp(minYAngle, maxYAngle, percentInside);
        needle.localRotation = Quaternion.Euler(0f, 0f, yAngle);

        if(percentInside > 0)
        {
            flag = 1;
            TriggerEvent();
        }
    }

    public void makeniddle()
    {
        minYAngle = 20f;
    }

    public void TriggerEvent()
    {
        Debug.Log("Blink");
        onTrigger?.Invoke(); // Calls all functions assigned in Inspector
    }
}
