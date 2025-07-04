using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneActiver : MonoBehaviour
{
    public GameObject gameobject;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
