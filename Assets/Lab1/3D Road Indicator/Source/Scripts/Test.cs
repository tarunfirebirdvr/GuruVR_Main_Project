using UnityEngine;
using RoadIndicator;

namespace RoadIndicator
{

    public class Test : MonoBehaviour
    {
        private void Start()
        {
            RoadIndicatorManager.OnPathCompleted.AddListener((location) =>
            {
                Debug.Log($"Arrived at {location.name}");
            });
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RoadIndicatorManager.SetIndicator("home");
            }
        }
    }

}