using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    bool placing = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        // This allows user to tap, rather than tap-and-drag
        placing = !placing;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (placing)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            // Do a raycast into the world that will _only hit the Spatial Mapping mesh._
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            RaycastHit hitInfo;  // Structure used to get information back from raycast.
            // FIXME: This 'if' needs to be the EXACTLY as written in the tutorial (copy/paste). 
            //    For some reason expanding over multiple lines like this does not work. WHY?
            //    Are there hidden whitespaces? YES: need no whitespace before comma-seperated newlines
            if (Physics.Raycast(headPosition,
                                gazeDirection,
                                out hitInfo,
                                30.0f,
                                SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.parent.position = hitInfo.point;

                // Rotate this object's parent object _to face the user._
                // Quaternions are used to represent rotations.
                //    Unity internally uses Quaternions to represent all rotations.
                Quaternion toQuat = Camera.main.transform.localRotation;  // The rotation of the transform relative to the parent transform's rotation
                toQuat.x = 0;
                toQuat.z = 0;
                // rotate this parent object to face the camera
                this.transform.parent.rotation = toQuat;
            }
        }
    }
}
