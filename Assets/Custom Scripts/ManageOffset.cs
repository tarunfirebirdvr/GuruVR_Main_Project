using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ManageOffset : MonoBehaviour
{
    public MultiAimConstraint aimConstraint;
    public float newXOffset;

    public void addoffset()
    {
        if (aimConstraint != null)
        {
            // Get the current data
            var data = aimConstraint.data;

            // Change the X component of the offset
            Vector3 offset = data.offset;
            offset.x = newXOffset;
            data.offset = offset;

            // Apply the modified data back to the constraint
            aimConstraint.data = data;
        }
    }
}
