using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SocketSnap : MonoBehaviour
{
    [Tooltip("Only objects with this tag can snap.")]
    public string targetTag = "Grabbable";

    [Tooltip("Should the object be locked in place?")]
    public bool lockInPlace = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        // Snap position and rotation to this socket
        other.transform.position = transform.position;
        other.transform.rotation = transform.rotation;

        // Lock the object if desired
        if (lockInPlace)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        // Unlock the object when it leaves
        if (lockInPlace)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}
