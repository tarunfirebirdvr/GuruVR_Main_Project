using UnityEngine;

public class BotManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) {
            if (collision.gameObject.CompareTag("AndroidPlayer"))
            {

            }
    } }
}
