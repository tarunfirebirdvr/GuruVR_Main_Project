using UnityEngine;
using UnityEngine.Events;

public class SocketIntractor : MonoBehaviour
{
    [Tooltip("Transform where the object will snap to.")]
    public Transform snapPoint;

    [Tooltip("Tag of objects that can be snapped.")]
    public string validTag = "Grabbable";

    [Tooltip("Should the object be locked after snapping?")]
    public bool lockInPlace = true;

    private GameObject currentObject = null;
    public UnityEvent onTriggerEnterEvent;
    private bool hasTriggered = false;
    public GameObject grab;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("SnapPost-" + snapPoint.rotation);
        if (currentObject != null) return; // Already holding something
        BoxCollider box = other.GetComponent<BoxCollider>();
        /*Rigidbody rb = other.GetComponent<Rigidbody>();
        Destroy(rb);*/
        if (box != null)
        {
            box.enabled = false;
            //  Debug.Log("BoxCollider on " + other.name + " has been disabled.");
        }
        if (other.CompareTag(validTag))
        {
            if (!hasTriggered)
            {
                grab.SetActive(false);  
                hasTriggered = true;
                GameObject obj = other.gameObject;
                Debug.Log("Post-" + obj.transform.rotation);
                obj.transform.position = snapPoint.transform.position;
                obj.transform.rotation = snapPoint.transform.rotation;
                onTriggerEnterEvent.Invoke();
                // SnapObject(currentObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentObject)
        {
          //  UnsnapObject();
        }
    }

    private void SnapObject(GameObject obj)
    {
        Debug.Log("Post-" + obj.transform.position);
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;

        // Optionally freeze the object
        if (lockInPlace)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                if (!hasTriggered)
                {
                    hasTriggered = true;
                    onTriggerEnterEvent.Invoke();
                }
            }

        }
        
    }
   
   
private void UnsnapObject()
    {
        if (currentObject != null && lockInPlace)
        {
            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        currentObject = null;
    }
}
