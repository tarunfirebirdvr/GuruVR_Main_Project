using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using TMPro;
using UnityEngine.SceneManagement;

public class AddressableScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string address = "Playground"; // Scene key in Addressables
    [SerializeField] private string remoteCatalogURL = "https://unitytest0.blob.core.windows.net/unitydevtest/testdir/Windows/catalog_0.1.0.json";

    [Header("UI")]
    [SerializeField] private TMP_Text logText;
    [SerializeField] private Image progressImage;

    private bool catalogLoaded = false;

    void Start()
    {
        // Load remote content catalog
        Addressables.LoadContentCatalogAsync(remoteCatalogURL).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                logText.text = " Remote catalog loaded.";
                catalogLoaded = true;
            }
            else
            {
                logText.text = " Failed to load remote catalog.";
            }
        };
    }

    void Update()
    {
        // Trigger scene load on key press
        if (Input.GetKeyDown(KeyCode.W) && catalogLoaded)
        {
            StartCoroutine(LoadAddressableSceneWithProgress(address));
        }
    }
    public void startscene()
    {
        StartCoroutine(LoadAddressableSceneWithProgress(address));
    }

    private System.Collections.IEnumerator LoadAddressableSceneWithProgress(string key)
    {
        var handle = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);

        while (!handle.IsDone)
        {
            float progress = handle.PercentComplete;
            logText.text = $"Loading Scene '{key}'... {Mathf.RoundToInt(progress * 100)}%";

            if (progressImage != null)
                progressImage.fillAmount = progress;

            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            logText.text = $"Scene '{key}' loaded!";
        }
        else
        {
            logText.text = $"Failed to load Addressable Scene: {key}";
        }
    }
}
