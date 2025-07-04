/*#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using UnityEngine;

namespace ReadyPlayerMe.PhotonSupport
{
    public class BasicMovement : MonoBehaviour
    {
        [SerializeField] private new GameObject camera;
        
        private Animator animator;
        private PhotonView photonView;
        
        private readonly static int WALK_ANIM = Animator.StringToHash("Walking");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();
            
            if (photonView.IsMine) camera.SetActive(true);
        }
        
        private void Update()
        {
            if (photonView.IsMine)
            {
                var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
                var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
            
                transform.Rotate(0, x, 0);
                transform.Translate(0, 0, z);

                animator.SetBool(WALK_ANIM, z != 0);
            }
        }
    }
}
#endif
*/
/*#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using UnityEngine;

namespace ReadyPlayerMe.PhotonSupport
{
    public class BasicMovement : MonoBehaviour
    {
        [SerializeField] private new GameObject camera;
        [SerializeField] private Joystick joystick; // Reference to the joystick UI
        [SerializeField] private GameObject joystickUI; // Reference to the joystick UI

        private Animator animator;
        private PhotonView photonView;

        private readonly static int WALK_ANIM = Animator.StringToHash("Walking");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();

            if (photonView.IsMine)
            {
                camera.SetActive(true);
                joystickUI.SetActive(true);
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                float x = joystick.Horizontal * Time.deltaTime * 150.0f;
                float z = joystick.Vertical * Time.deltaTime * 3.0f;

                transform.Rotate(0, x, 0);
                transform.Translate(0, 0, z);

                animator.SetBool(WALK_ANIM, Mathf.Abs(z) > 0.01f);
            }
        }
    }
}
#endif*/

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
using UnityEngine;

namespace ReadyPlayerMe.PhotonSupport
{
    public class BasicMovement : MonoBehaviour
    {
        [SerializeField] private new GameObject camera;
        [SerializeField] private Joystick joystick; // Reference to the joystick UI
        [SerializeField] private GameObject joystickUI; // Reference to the joystick UI

        private Animator animator;
        private PhotonView photonView;

        private readonly static int WALK_ANIM = Animator.StringToHash("Walking");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            photonView = GetComponent<PhotonView>();

            if (photonView.IsMine)
            {
                camera.SetActive(true);
                joystickUI.SetActive(true);
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            // Get input from both sources
            float keyboardHorizontal = Input.GetAxis("Horizontal");
            float keyboardVertical = Input.GetAxis("Vertical");

            float joystickHorizontal = joystick != null ? joystick.Horizontal : 0f;
            float joystickVertical = joystick != null ? joystick.Vertical : 0f;

            // Combine inputs
            float horizontal = keyboardHorizontal + joystickHorizontal;
            float vertical = keyboardVertical + joystickVertical;

            // Clamp values to [-1, 1]
            horizontal = Mathf.Clamp(horizontal, -1f, 1f);
            vertical = Mathf.Clamp(vertical, -1f, 1f);

            // Apply movement
            float rotation = horizontal * Time.deltaTime * 150.0f;
            float movement = vertical * Time.deltaTime * 3.0f;

            transform.Rotate(0, rotation, 0);
            transform.Translate(0, 0, movement);

            // Set walking animation
            animator.SetBool(WALK_ANIM, Mathf.Abs(movement) > 0.01f);
        }
    }
}
#endif
