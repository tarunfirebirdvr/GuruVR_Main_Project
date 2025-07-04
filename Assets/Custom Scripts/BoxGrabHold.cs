using UnityEngine;

public class BoxGrabHold : MonoBehaviour
{
    public BoxCollider boxA;
    public BoxCollider boxB;
    public Transform needle;

    public float minYAngle = 0f;    // Needle angle when 0% inside
    public float maxYAngle = 90f;   // Needle angle when 100% inside

    private Vector3 lastBoxBPosition;
    private bool isFirstFrame = true;

    void Update()
    {
        Vector3 currentPos = boxB.transform.position;

        // Check if boxB has moved since last frame
        bool isMoving = !isFirstFrame && currentPos != lastBoxBPosition;

        if (isMoving)
        {
            Bounds boundsA = boxA.bounds;
            Bounds boundsB = boxB.bounds;

            float xOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.x, boundsB.max.x) - Mathf.Max(boundsA.min.x, boundsB.min.x));
            float yOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.y, boundsB.max.y) - Mathf.Max(boundsA.min.y, boundsB.min.y));
            float zOverlap = Mathf.Max(0, Mathf.Min(boundsA.max.z, boundsB.max.z) - Mathf.Max(boundsA.min.z, boundsB.min.z));

            float intersectionVolume = xOverlap * yOverlap * zOverlap;
            float volumeA = boundsA.size.x * boundsA.size.y * boundsA.size.z;

            float percentInside = (intersectionVolume / volumeA) * 100f;
            percentInside = Mathf.Clamp01(percentInside / 100f);

            float yAngle = Mathf.Lerp(minYAngle, maxYAngle, percentInside);
            needle.localRotation = Quaternion.Euler(0f, yAngle, 0f);
        }

        lastBoxBPosition = currentPos;
        isFirstFrame = false;
    }
}
