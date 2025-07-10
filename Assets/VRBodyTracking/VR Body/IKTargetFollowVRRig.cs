/*using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0,1)]
    public float turnSmoothness = 0.1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    [SerializeField] PhotonView PV;

    // Update is called once per frame
    void LateUpdate()
    {
        if (PV.IsMine)
        {
            transform.position = head.ikTarget.position + headBodyPositionOffset;
            float yaw = head.vrTarget.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
        
    }
}*/

/*using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;            // Primary (e.g., hand)
    public Transform fallbackVrTarget;    // Fallback (e.g., controller)
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        Transform activeTarget = (vrTarget != null && vrTarget.gameObject.activeSelf)
            ? vrTarget
            : fallbackVrTarget;

        if (activeTarget != null && ikTarget != null)
        {
            ikTarget.position = activeTarget.TransformPoint(trackingPositionOffset);
            ikTarget.rotation = activeTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    [SerializeField] PhotonView PV;

    void LateUpdate()
    {
        if (PV != null && PV.IsMine)
        {
            // Get the active head transform (typically head.vrTarget is always present)
            Transform activeHeadTarget = (head.vrTarget != null && head.vrTarget.gameObject.activeSelf)
                ? head.vrTarget
                : head.fallbackVrTarget;

            if (activeHeadTarget != null)
            {
                // Update body position based on head
                transform.position = head.ikTarget.position + headBodyPositionOffset;

                float yaw = activeHeadTarget.eulerAngles.y + headBodyYawOffset;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yaw, 0), turnSmoothness);
            }

            // Update IK mapping
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
}
*/
using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;                 // Primary tracked hand/head
    public Transform fallbackVrTarget;         // Fallback controller
    public Transform ikTarget;                 // IK target to move
    public Vector3 trackingPositionOffset;     // Positional offset
    public Vector3 trackingRotationOffset;     // Rotational offset
    public Transform idlePosition;             // Idle/default position when nothing is tracked

    public void Map()
    {
        Transform activeTarget = null;

        if (vrTarget != null && vrTarget.gameObject.activeSelf)
        {
            activeTarget = vrTarget;
        }
        else if (fallbackVrTarget != null && fallbackVrTarget.gameObject.activeSelf)
        {
            activeTarget = fallbackVrTarget;
        }

        if (activeTarget != null && ikTarget != null)
        {
            // Map to active tracked input
            ikTarget.position = activeTarget.TransformPoint(trackingPositionOffset);
            ikTarget.rotation = activeTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
        else if (idlePosition != null && ikTarget != null)
        {
            // Fallback to idle position
            ikTarget.position = idlePosition.position;
            ikTarget.rotation = idlePosition.rotation;
        }
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0f, 1f)]
    public float turnSmoothness = 0.1f;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset = 0f;

    [SerializeField] private PhotonView PV;

    void LateUpdate()
    {
        if (PV != null && PV.IsMine)
        {
            // Get active head tracking reference
            Transform headRef = head.vrTarget != null && head.vrTarget.gameObject.activeSelf
                ? head.vrTarget
                : head.fallbackVrTarget;

            if (headRef != null && head.ikTarget != null)
            {
                // Move avatar to follow head tracking
                transform.position = head.ikTarget.position + headBodyPositionOffset;

                float targetYaw = headRef.eulerAngles.y + headBodyYawOffset;
                Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothness);
            }

            // Map head and hands
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
}
