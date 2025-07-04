using UnityEngine;

public class CubeClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // Check if this GameObject was hit
                {
                    Play();
                    this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
                }
            }
        }
    }

    void Play()
    {
        Debug.Log("Cube clicked! Playing function...");
        // Your custom code here, e.g., play sound, animation, or logic
    }
}
