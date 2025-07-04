using Photon.Pun;
using UnityEngine;

public class NetworkAvatar : MonoBehaviourPun
{
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _Loco;
    [SerializeField] private GameObject _HandPos;

    public GameObject loco;
    public GameObject origin;
    public Vector3 post;

    void Start()
    {
        if (!photonView.IsMine)
        {
            if (_mainCamera != null)
            {
                _mainCamera.SetActive(false);
                _Loco.SetActive(false);
                _HandPos.SetActive(false);
            }
           
        }
    }

    /*private void Update()
    {
        if (!photonView.IsMine)
        {
            if(origin != null)
             origin.transform.position = loco.transform.position;
        }
        else
        {
             loco.transform.position = origin.transform.position;
        }
    }*/
}
