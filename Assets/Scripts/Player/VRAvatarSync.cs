using UnityEngine;
using Photon.Pun;

public class VRAvatarSync : MonoBehaviourPun
{
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    void Start()
    {
        // Disable tracking script for remote players
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    void LateUpdate()
    {
        // Only apply if this is the local player's avatar
        if (!photonView.IsMine) return;

        if (headBone && headTarget)
        {
            headBone.position = headTarget.position;
            headBone.rotation = headTarget.rotation;
        }

        if (leftHandBone && leftHandTarget)
        {
            leftHandBone.position = leftHandTarget.position;
            leftHandBone.rotation = leftHandTarget.rotation;
        }

        if (rightHandBone && rightHandTarget)
        {
            rightHandBone.position = rightHandTarget.position;
            rightHandBone.rotation = rightHandTarget.rotation;
        }
    }
}
