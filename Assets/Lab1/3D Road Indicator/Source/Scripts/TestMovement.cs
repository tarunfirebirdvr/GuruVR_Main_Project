using UnityEngine;

namespace RoadIndicator
{
    public class TestMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 100f;

        void Update()
        {
            // Move forward/backward and strafe left/right
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // Rotate left/right
            float rotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotation);
        }
    }
}
