using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Renderer targetRenderer;

    void Start()
    {
        // Optional: Automatically get the Renderer if not assigned
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();
    }

    // Call this function to change the color to yellow
    public void SetYellow()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = Color.blue;
        }
        else
        {
            Debug.LogWarning("Renderer not assigned.");
        }
    }
}
