using UnityEngine;

public class PorterManager : MonoBehaviour
{
    [SerializeField] GameObject ObjectToEnable;
    [SerializeField] GameObject ObjectToDisable;   
    [SerializeField] GameObject Player;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player = other.gameObject;
        if (other.gameObject.CompareTag("Player"))
        {
            if (ObjectToEnable != null)
            {
                ObjectToEnable.SetActive(true);
            }
            if (ObjectToDisable != null)
            {
                ObjectToDisable.SetActive(false);
            }
        }

    }

    public void TeleportToLearningArea()
    {
        Player.transform.position = new Vector3(-1.46f, Player.transform.position.y, -129.07f);
    }
}
