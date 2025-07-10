using System.Collections;
using UnityEngine;

public class GateOpenScript : MonoBehaviour
{
    public float openAngle = 90f;         // How much to rotate on Y-axis
    public float openSpeed = 2f;          // Speed of rotation
    public bool isOpen = false;           // Track state

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
        
    }
    public void openGate()
    {
        StartCoroutine(OpenGateWithDelay());
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * openSpeed);
        }
    }

    // Call this method to toggle the gate
    public void ToggleGate()
    {
        isOpen = !isOpen;
    }

    private IEnumerator OpenGateWithDelay()
    {
        yield return new WaitForSeconds(7f);
        ToggleGate();
    }


    // Optional: Trigger via collision
    private void OnTriggerEnter(Collider other)
    {
       // if (other.CompareTag("Player"))
        {
            ToggleGate(); // Open when player enters trigger
        }
    }
}
