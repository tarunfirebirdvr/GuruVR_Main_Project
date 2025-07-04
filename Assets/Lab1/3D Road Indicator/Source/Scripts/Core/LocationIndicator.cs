using UnityEngine;

namespace RoadIndicator
{
    public class LocationIndicator : MonoBehaviour
    {
        [Tooltip("Unique identifier for the location")]
        public string id;
        [Tooltip("If true, means that their are more than one location using the same id. the location will be generated to the nearest one from the player position.")]
        public bool duplicate;
        [Tooltip("Name of the location, used for display purposes.")]
        public new string name;
        [Tooltip("Description of the location, used for display purposes.")]
        public string description;

    }
}