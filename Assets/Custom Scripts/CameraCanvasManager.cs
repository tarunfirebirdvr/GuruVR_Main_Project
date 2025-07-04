using Unity.VisualScripting;
using UnityEngine;

public class CameraCanvasManager : MonoBehaviour
{
    /*public GameObject cam;
    private Canvas _canvas;
    private Camera _currentCamera;
    void Start()
    {
        
    }
    private void Update()
    {
        if (!cam.activeSelf)
        {
            AssignCamera();
        }

    }
    private void AssignCamera()
    {
        Camera newCamera = GetActiveCamera();

        if (newCamera != null)
        {
            _canvas.worldCamera = newCamera;
            _currentCamera = newCamera;
            Debug.Log($"[CanvasCameraAssigner] Assigned new camera: {newCamera.name} to canvas: {_canvas.name}");
        }
        else
        {
            Debug.LogWarning("[CanvasCameraAssigner] No active camera found.");
        }
    }
    private Camera GetActiveCamera()
    {
       Camera mycam = GameObject.Find("Camera").GetComponent<Camera>();

                return mycam;
        

    }
}*/
    private Canvas _canvas;
    private Camera _currentCamera;

    void Start()
    {
        _canvas = GetComponent<Canvas>();

        if (_canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("Canvas is not in World Space. Disabling script.");
            enabled = false;
        }
    }

    void Update()
    {
        Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();

        if (mainCam != null && _canvas.worldCamera != mainCam)
        {
            _canvas.worldCamera = mainCam;
            _currentCamera = mainCam;
            Debug.Log($"Assigned new MainCamera: {mainCam.name} to canvas: {_canvas.name}");
        }
    }
}
