using UnityEngine;


namespace RoadIndicator
{
    public class DestinationPoint : MonoBehaviour
    {
        [HideInInspector] public LocationIndicator location;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerIndicator _))
            {
                RoadIndicatorManager.OnPathCompleted.Invoke(location);
                Destroy(gameObject);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }

}