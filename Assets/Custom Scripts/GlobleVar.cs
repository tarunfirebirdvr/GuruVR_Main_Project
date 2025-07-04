using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobleVar : MonoBehaviour
{
    public static bool isreturn = false;
    public static GlobleVar Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes it persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturntomainScene()
    {
        isreturn = true;
    }
}
