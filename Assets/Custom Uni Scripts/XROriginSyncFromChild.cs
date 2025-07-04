using Photon.Pun;
using UnityEngine;

public class XROriginSyncFromChild : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Transform xrOrigin; // Assign the XR Origin in Inspector

    private Vector3 receivedPosition;
    private Quaternion receivedRotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (xrOrigin == null) return;

        if (stream.IsWriting) // This is the local player
        {
            stream.SendNext(xrOrigin.position);
            stream.SendNext(xrOrigin.rotation);
        }
        else // This is a remote player
        {
            receivedPosition = (Vector3)stream.ReceiveNext();
            receivedRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine && xrOrigin != null)
        {
            xrOrigin.position = Vector3.Lerp(xrOrigin.position, receivedPosition, Time.deltaTime * 10);
            xrOrigin.rotation = Quaternion.Lerp(xrOrigin.rotation, receivedRotation, Time.deltaTime * 10);
        }
    }
}
