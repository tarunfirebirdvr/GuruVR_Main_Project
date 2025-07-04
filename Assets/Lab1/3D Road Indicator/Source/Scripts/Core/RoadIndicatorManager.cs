using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


namespace RoadIndicator
{
    public class RoadIndicatorManager : MonoBehaviour
    {
        public static RoadIndicatorManager Instance { get; private set; }
        [Header("(Do not change unless you know what you are doing)")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private DestinationPoint destinationPoint;
        [Header("Drag the player indicator here. If not set, the system will look for the first object with PlayerIndicator script attached.")]
        [SerializeField] private PlayerIndicator player;
        [SerializeField] private float pathWidth = 1f;
        [SerializeField, Tooltip("The radius of area that player complete the path when enter")] private float destinationPointRadius = 10f;
        // [SerializeField, Tooltip("The offset of destination point from the center of targeted location")] private float destinationPointSpawnOffset = 0f;
        private NavMeshPath path;
        public static UnityEvent<LocationIndicator> OnPathCompleted;
        public static UnityEvent<LocationIndicator> OnPathCanceled;
        private List<LocationIndicator> locations;
        private DestinationPoint currentDestinationPoint;
        private void Awake()
        {
            #region Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("There can only be one ToastifyManager in the scene");
                Destroy(this);
            }
            #endregion
            // Initialization
            path = new();
            locations = new();
            OnPathCompleted = new();
            OnPathCanceled = new();
        }
        private void Start()
        {
            // Finding all LocationIndicator objects in the scene and adding them to the list
            locations = FindObjectsOfType<LocationIndicator>().ToList();
        }

        /// <summary>
        /// This method is responsible for setting an indicator path based on the provided ID. It retrieves the LocationIndicator object associated with the provided ID and initiates the path calculation from the player's current position to the destination.
        /// <param name="id">LocationIndicator id</param>   
        /// </summary>
        public static void SetIndicator(string id)
        {
            LocationIndicator target = Instance.GetLocation(id);
            if (target == null)
            {
                Debug.LogError($"Path with id:[{id}] not found");
                return;
            }
            Instance.StartPath(target);
        }
        /// <summary>
        /// This method cancels an indicator path. If an ID is provided, it invokes the path cancellation event for the specified location. If no ID is provided, it clears the line renderer and destroys the current destination point.
        /// </summary>
        /// <param name="id">LocationIndicator id</param>
        public static void CancelIndicator(string id = null)
        {
            if (id != null)
            {
                LocationIndicator target = Instance.GetLocation(id);
                OnPathCanceled.Invoke(target);
            }
            else
            {
                Instance.lineRenderer.positionCount = 0;
                if (Instance.currentDestinationPoint != null)
                {
                    Destroy(Instance.currentDestinationPoint.gameObject);
                    Instance.currentDestinationPoint = null;
                }
            }
        }

        // Start a path to the provided destination
        private void StartPath(LocationIndicator destination)
        {
            // Clearing the current path and initializing a new NavMesh path
            path?.ClearCorners();
            path = new();
            lineRenderer.startWidth = pathWidth;
            // Calculating the NavMesh path from player's position to the destination
            if (!NavMesh.CalculatePath(player.transform.position, destination.transform.position, NavMesh.AllAreas, path))
            {
                Debug.LogError($"Path to {destination.name} not found");
                return;
            }
            if (path.corners == null || path.corners.Length == 0)
            {
                Debug.LogError($"Path to {destination.name} not found");
                return;
            }

            lineRenderer.positionCount = path.corners.Length;
            Vector3[] vectors = path.corners;


            for (int i = 0; i < path.corners.Length; i++)
            {
                // Adjusting y-coordinate to avoid clipping with ground
                var vector = path.corners[i];
                vector.y += 0.2f;
                vectors[i] = vector;
            }

            // If there is a current destination point, destroy it and remove all listeners from the path completion and cancellation events
            if (currentDestinationPoint != null)
            {
                Destroy(currentDestinationPoint.gameObject);
                OnPathCompleted.RemoveAllListeners();
                OnPathCanceled.RemoveAllListeners();
            }
            // Spawn the destination point in the destination location.
            currentDestinationPoint = Instantiate(destinationPoint, destination.transform.position, Quaternion.identity);
            // currentDestinationPoint.transform.localScale = Vector3.one * destinationPointRadius;
            // [TODO] currentDestinationPoint.transform.position += Vector3.forward * destinationPointSpawnOffset;

            // Set the scale of the destination point to the destination point radius.
            currentDestinationPoint.transform.localScale = Vector3.one * destinationPointRadius;

            // Assign the destination location to the destination point.
            currentDestinationPoint.location = destination;

            // Set the corners positions to the line renderer to draw the path.
            lineRenderer.SetPositions(vectors);

            OnPathCompleted.AddListener((LocationIndicator location) =>
             {
                 if (location != destination) return;
                 lineRenderer.positionCount = 0;
                 if (currentDestinationPoint != null)
                 {
                     Destroy(currentDestinationPoint.gameObject);
                     currentDestinationPoint = null;
                 }
             });
            OnPathCanceled.AddListener((LocationIndicator location) =>
            {
                if (location != destination) return;
                lineRenderer.positionCount = 0;
                if (currentDestinationPoint != null)
                {
                    Destroy(currentDestinationPoint.gameObject);
                    currentDestinationPoint = null;
                }
            });
        }




        private LocationIndicator GetLocation(string id)
        {
            LocationIndicator target = locations.Find(x => x.id == id);
            if (target == null)
            {
                Debug.LogError("Location with id " + id + " not found");
            }
            if (target.duplicate)
            {
                target = locations.OrderBy(x => Vector3.Distance(player.transform.position, x.transform.position)).FirstOrDefault();
            }
            return target;
        }
    }
}
