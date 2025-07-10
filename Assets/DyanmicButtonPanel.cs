using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyanmicButtonPanel : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string name;
        public string link;
    }

    public GameObject buttonPrefab; // Assign in inspector
    public Transform contentParent; // Assign Content object of ScrollView

    // Dummy API Data
    private List<ItemData> dummyData = new List<ItemData>
    {
        new ItemData { name = "Module 1", link = "https://example.com/module1" },
        new ItemData { name = "Module 2", link = "https://example.com/module2" },
        new ItemData { name = "Module 3", link = "https://example.com/module3" }
    };

    void Start()
    {
        GenerateButtons(dummyData);
        GenerateButtons(dummyData);
        GenerateButtons(dummyData);
    }

    void GenerateButtons(List<ItemData> items)
    {
        foreach (var item in items)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentParent);
            newButton.GetComponentInChildren<Text>().text = item.name;

            // Capture item.link in a local variable to avoid closure issues
            string linkCopy = item.link;
            newButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(linkCopy));
        }
    }

    void OnButtonClick(string link)
    {
        Debug.Log("Button clicked with link: " + link);
        // TODO: Use the link to load module, open scene, web request, etc.
    }
}
