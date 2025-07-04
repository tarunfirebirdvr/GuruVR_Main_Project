using UnityEngine;
using UnityEngine.Events;

public class isGrabedTrue : MonoBehaviour
{
    public GameObject targetObject;  // Object to activate (can be itself)
    private Vector3 initialPosition;
    private bool hasMoved = false;
    public UnityEvent onSlideStart;
    void Start()
    {
        if (targetObject == null)
            targetObject = gameObject; // If no target specified, use this GameObject

        targetObject.SetActive(false); // Start deactivated
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!hasMoved && transform.position != initialPosition)
        {
            targetObject.SetActive(true);
            hasMoved = true; // Only activate once
            onSlideStart?.Invoke();

        }
    }
    private void OnDisable()
    {
        targetObject.SetActive(false);
        initialPosition = transform.position;
    }

}
