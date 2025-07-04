using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPun
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;

    private CharacterController characterController;
    private XROrigin xrOrigin;

    private Vector2 inputAxis;

    [Header("XR Input Action Reference")]
    public InputActionProperty moveInput; // Assign this in inspector (from your Input Action asset)

    [Header("Android Settings")]
    public bool isAndroid = false;
    [SerializeField] Camera PlayerCameraPhoton;

    private void Start()
    {
        
        if (!photonView.IsMine)
        {
            PlayerCameraPhoton.enabled = false;
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            Destroy(this); // Only control local player
            return;
        }
      //  PlayerCameraPhoton.enabled = true;

        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();

#if UNITY_ANDROID && !UNITY_EDITOR
        isAndroid = true;
#endif
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (isAndroid)
        {
            HandleTouchMovement();
        }
        else
        {
            inputAxis = moveInput.action.ReadValue<Vector2>();

            Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);
            Vector3 headYaw = new Vector3(xrOrigin.Camera.transform.forward.x, 0, xrOrigin.Camera.transform.forward.z).normalized;
            Quaternion headRotation = Quaternion.LookRotation(headYaw);

            Vector3 movement = headRotation * direction;
            characterController.Move(movement * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleTouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            Vector2 touchPos = touch.position;
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 dir = (touchPos - screenCenter).normalized;

            Vector3 move = new Vector3(dir.x, 0, dir.y);
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }
    }
}
