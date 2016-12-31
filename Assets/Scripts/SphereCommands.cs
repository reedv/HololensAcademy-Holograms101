using UnityEngine;

public class SphereCommands : MonoBehaviour
{
    Vector3 originalPosition;

    // Use this for initialization
    void Start()
    {
        // Save the original local position of the sphere when the app starts.
        originalPosition = this.transform.localPosition;
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        // If the sphere has no Rigidbody component, add one to enable physics.
        if (!this.GetComponent<Rigidbody>())
        {
            // Adding a Rigidbody component to an object will put its motion under 
            // the control of Unity's physics engine
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();

            // Collisions will be detected for any static mesh geometry in the path 
            // of this Rigidbody, even if the collision occurs between two FixedUpdate steps
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    // Called by SpeechManager when the user says the "Reset world" command
    // see Scripts\SpeechManager.cs
    void OnReset()
    {
        // If the sphere has a Rigidbody component (thus governed by unity physics engin), 
        // remove it to disable physics.
        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            DestroyImmediate(rigidbody);
        }

        // Put the sphere back into its original local position.
        this.transform.localPosition = originalPosition;
    }

    // Called by SpeechManager when the user says the "Drop sphere" command
    // see Scripts\SpeechManager.cs
    void OnDrop()
    {
        // Just do the same logic as a Select gesture.
        this.OnSelect();
    }
}
