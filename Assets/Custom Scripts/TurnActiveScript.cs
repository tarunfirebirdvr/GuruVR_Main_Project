using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnActiveScript : MonoBehaviour
{
    [Tooltip("Event triggered when local X rotation is exactly 140 degrees.")]
    public UnityEvent onXRotation140;

    [Tooltip("Tolerance for comparing rotation angles.")]
    public float tolerance = 1f;

    public bool hasTriggered = false;

    void Update()
    {
        float xRotation = transform.localEulerAngles.x;

        // Normalize angle to range [0, 360]
        xRotation = (xRotation + 360) % 360 + 100;
        Debug.Log(xRotation);
        if (!hasTriggered && Mathf.Abs(xRotation - 140f) <= tolerance)
        {
            hasTriggered = true;
            onXRotation140.Invoke();
        }

        // Reset trigger if rotation moves away (optional)
       /* if (hasTriggered && Mathf.Abs(xRotation - 140f) > tolerance)
        {
            hasTriggered = false;
        }*/
    }
}
