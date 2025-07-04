using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QRScannerConfig : MonoBehaviour
{
    [Tooltip("The name of the Text UI element to watch")]
    public string targetTextObjectName = "QRText";

    [Tooltip("The name of the scene to load when text changes")]
    public string sceneToLoad = "Working 8";

    private TextMeshProUGUI targetText;
    private string lastTextValue;
    public int flag = 0;
    void Start()
    {

        // Find the text object by name

    }

        void Update()
    {
        GameObject targetObj = GameObject.Find(targetTextObjectName);

        if (targetObj != null)
        {
            targetText = targetObj.GetComponent<TextMeshProUGUI>();

            if (targetText != null)
            {
                lastTextValue = targetText.text;
            }
            else
            {
                Debug.LogError("No Text component found on GameObject: " + targetTextObjectName);
            }
        }
        else
        {
            Debug.LogError("No GameObject found with name: " + targetTextObjectName);
        }
        if (targetText == null)
            return;

        if (targetText.text != lastTextValue && flag == 0)
        {
            flag = 1;
            Debug.Log("Text changed! Loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
