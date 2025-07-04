using UnityEngine;

public class universityBtnManager : MonoBehaviour
{

    public GateOpenScript LeftGate;
    public GateOpenScript RightGate;
    public GameObject ExplorePorter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ExplorePorter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LeftGate.isOpen = true;
            RightGate.isOpen = true;
            ExplorePorter.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
