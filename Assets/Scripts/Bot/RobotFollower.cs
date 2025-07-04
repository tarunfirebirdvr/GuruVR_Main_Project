using UnityEngine;

public class RobotFollower : MonoBehaviour
{
    public PhotonManager photonmanager;  // Reference to your PhotonManager
    public Transform lerpTarget;         // This will be the destination for Lerp movement (set from Inspector)
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float minDistance = 1.5f;

    public float teleportRadius = 5f;
    public LayerMask obstacleLayer;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        if (photonmanager != null && photonmanager.Instantiated_Player != null)
        {
            player = photonmanager.Instantiated_Player.transform;
        }
        else
        {
            Debug.LogError("PhotonManager or Instantiated_Player not assigned!");
        }

        rb = GetComponent<Rigidbody>();

        if (lerpTarget == null)
        {
            Debug.LogWarning("Lerp Target is not assigned. Robot will not move.");
        }
    }

    void Update()
    {
        if (lerpTarget == null) return;

        Vector3 targetPos = lerpTarget.position;
        Vector3 direction = (targetPos - transform.position);

        if (direction.magnitude > minDistance)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            Vector3 safePosition = FindSafePosition();
            rb.MovePosition(safePosition);
        }
    }

    Vector3 FindSafePosition()
    {
        int maxAttempts = 20;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * teleportRadius;
            randomOffset.y = 0;
            Vector3 candidatePosition = (player != null ? player.position : transform.position) + randomOffset;

            if (!Physics.CheckSphere(candidatePosition, 0.5f, obstacleLayer))
            {
                return candidatePosition;
            }
        }

        Debug.LogWarning("No safe position found. Returning to player or self.");
        return player != null ? player.position : transform.position;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (lerpTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lerpTarget.position, 0.3f);
            Gizmos.DrawLine(transform.position, lerpTarget.position);
        }

        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, teleportRadius);
        }
    }
#endif
}
