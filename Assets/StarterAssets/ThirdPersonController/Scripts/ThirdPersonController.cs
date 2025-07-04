using UnityEngine;


using System.Collections;


#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using Photon.Pun;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviourPun
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;

        [Header("Virtual Joysticks")]
        public Joystick leftJoystick;  // Movement
        public Joystick rightJoystick; // Camera

        [Header("Joystick Rotation Settings")]
        [Tooltip("Rotation speed applied when using the left joystick's horizontal axis.")]
        public float RotationSpeed = 100.0f;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private float _speed;
        private float _animationBlend;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _JoyStickCanvas;

        private const float _threshold = 0.01f;
        private bool _hasAnimator;
        private int flag = 0;
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                if (_mainCamera != null) _mainCamera.SetActive(false);
                _JoyStickCanvas.SetActive(false);
                if (CinemachineCameraTarget != null) CinemachineCameraTarget.SetActive(false);
                return;
            }

            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            if (!photonView.IsMine) return;

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            //  StartCoroutine(DelayedAction());
        }
        /* IEnumerator DelayedAction()
         {
             Debug.Log("Action will happen in 2 seconds...");
             yield return new WaitForSeconds(5f); // Delay here
             Debug.Log("Action executed after delay.");
             if (flag == 0)
             {
                 flag = 1;
                 Gravity = -10;
             }
         }*/

        private void Update()
        {
            if (!photonView.IsMine) return;

            _hasAnimator = TryGetComponent(out _animator);

            GroundedCheck();
            JumpAndGravity();
            Move();
        }

        private void LateUpdate()
        {
            if (!photonView.IsMine) return;

            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            Vector2 lookInput = rightJoystick != null ? rightJoystick.Direction : Vector2.zero;

            if (lookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += lookInput.x * Time.deltaTime * 150f;
                _cinemachineTargetPitch += lookInput.y * Time.deltaTime * 150f;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }
        /*private void Move()
        {
            Vector2 joystickInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
            Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 moveInput = joystickInput != Vector2.zero ? joystickInput : keyboardInput;

            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (moveInput == Vector2.zero) targetSpeed = 0.0f;

            float inputMagnitude = moveInput.magnitude;
            _speed = Mathf.Lerp(_speed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            Vector3 moveDir = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

            Vector3 velocity = moveDir * (_speed * Time.deltaTime) + Vector3.up * _verticalVelocity * Time.deltaTime;

            Debug.Log("Moving with velocity: " + velocity);

            _controller.Move(velocity);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _speed);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }*/
        /* private void Move()
         {
             Vector2 joystickInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
             Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
             Vector2 moveInput = joystickInput != Vector2.zero ? joystickInput : keyboardInput;

             float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
             if (moveInput == Vector2.zero) targetSpeed = 0.0f;

             float inputMagnitude = moveInput.magnitude;
             _speed = Mathf.Lerp(_speed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

             Vector3 moveDir = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

             // ✅ Rotate toward move direction if there's input
             if (moveDir.magnitude > 0.1f)
             {
                 // Remove vertical component for clean rotation
                 Vector3 direction = new Vector3(moveDir.x, 0f, moveDir.z);
                 Quaternion targetRotation = Quaternion.LookRotation(direction);
                 transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSmoothTime * Time.deltaTime);
             }

             Vector3 velocity = moveDir * (_speed * Time.deltaTime) + Vector3.up * _verticalVelocity * Time.deltaTime;

             _controller.Move(velocity);

             if (_hasAnimator)
             {
                 _animator.SetFloat(_animIDSpeed, _speed);
                 _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
             }
         }*/

        private void Move()
        {
            Vector2 joystickInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
            Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 moveInput = joystickInput.sqrMagnitude > 0.1f ? joystickInput : keyboardInput;
         //   Debug.Log(joystickInput.sqrMagnitude);
            // Vector2 moveInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
            /*   Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
               Vector2 joystickInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
               Vector2 moveInput = joystickInput != Vector2.zero ? joystickInput : keyboardInput;
               if (leftJoystick != null && leftJoystick.Direction.sqrMagnitude > 0.1f)
                   moveInput = leftJoystick != null ? leftJoystick.Direction : Vector2.zero;
               else
                   moveInput = GetKeyboardMove();*/
            //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            //Debug.Log(keyboardInput);
            /**//* Vector2 moveInput = Vector2.zero;
            if (leftJoystick != null && leftJoystick.Direction.sqrMagnitude > 0.001f)
                moveInput = leftJoystick.Direction;
            else
                moveInput = GetKeyboardMove();*//**/
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (moveInput == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = moveInput.magnitude;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Apply rotation from left joystick X
            if (moveInput != Vector2.zero)
            {
                float rotationAmount = moveInput.x * RotationSpeed * Time.deltaTime;
                transform.Rotate(0f, rotationAmount, 0f);
            }

            // Move forward/backward in the direction the player is facing
            Vector3 moveDirection = transform.forward * moveInput.y;
            _controller.Move(moveDirection.normalized * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }


        }

        /* private void JumpAndGravity()
         {
             if (Grounded)
             {
                 _fallTimeoutDelta = FallTimeout;

                 if (_hasAnimator)
                 {
                     _animator.SetBool(_animIDJump, false);
                     _animator.SetBool(_animIDFreeFall, false);
                 }

                 if (_verticalVelocity < 0.0f)
                 {
                     _verticalVelocity = -2f;
                 }

                 if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                 {
                     _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                     if (_hasAnimator)
                     {
                         _animator.SetBool(_animIDJump, true);
                     }
                 }

                 if (_jumpTimeoutDelta >= 0.0f)
                 {
                     _jumpTimeoutDelta -= Time.deltaTime;
                 }
             }
             else
             {
                 _jumpTimeoutDelta = JumpTimeout;

                 if (_fallTimeoutDelta >= 0.0f)
                 {
                     _fallTimeoutDelta -= Time.deltaTime;
                 }
                 else
                 {
                     if (_hasAnimator)
                     {
                         _animator.SetBool(_animIDFreeFall, true);
                     }
                 }

                 _input.jump = false;
             }

             if (_verticalVelocity < _terminalVelocity)
             {
                 _verticalVelocity += Gravity * Time.deltaTime;
             }
         }*/
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // Reset fall timeout when grounded
                _fallTimeoutDelta = FallTimeout;

                // Reset animator states
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // Small downward force to keep grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Skip jump input — disabled
                // if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                // {
                //     _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                //
                //     if (_hasAnimator)
                //     {
                //         _animator.SetBool(_animIDJump, true);
                //     }
                // }

                // Countdown jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // Reset jump timeout when not grounded
                _jumpTimeoutDelta = JumpTimeout;

                // Handle free fall
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // Clear jump input while airborne
                _input.jump = false;
            }

            // Apply gravity
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }


        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = Grounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
        // NEW – quick keyboard sampler
        private static Vector2 GetKeyboardMove()
        {
#if ENABLE_INPUT_SYSTEM
            // New Input System
            var kb = Keyboard.current;
            if (kb == null) return Vector2.zero;

            float x = (kb.aKey.isPressed ? -1f : 0f) + (kb.dKey.isPressed ? 1f : 0f);
            float y = (kb.sKey.isPressed ? -1f : 0f) + (kb.wKey.isPressed ? 1f : 0f);
            return new Vector2(x, y).normalized;
#else
    // Old Input Manager
    return new Vector2(Input.GetAxisRaw("Horizontal"),
                       Input.GetAxisRaw("Vertical")).normalized;
#endif
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
