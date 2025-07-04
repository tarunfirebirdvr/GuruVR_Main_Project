using UnityEngine;

public class Debugger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        // Disable Unity’s built-in debug overlay (only in builds)
#if !UNITY_EDITOR
    Debug.unityLogger.logEnabled = false;
#endif
    }
}
