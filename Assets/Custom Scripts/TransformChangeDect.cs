using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformChangeDect : MonoBehaviour
{
    public UnityEvent onTransformChanged;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private bool hasTriggered = false;
    public bool IsMoved => hasTriggered;
    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {

        if (transform.position != lastPosition || transform.rotation != lastRotation)
        {
            if (!hasTriggered)
            {
                hasTriggered = true;
                onTransformChanged?.Invoke();

                lastPosition = transform.position;
                lastRotation = transform.rotation;
            }
        }
    }
}
