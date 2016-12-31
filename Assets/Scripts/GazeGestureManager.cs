using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        /*
         GestureRecognizer performs only the minimal disambiguation between the set of 
         gestures that you request. For example, if you request just Tap, the user may hold 
         their finger down as long as they like and a Tap will still occur when the user 
         releases their finger.
         */
        recognizer = new GestureRecognizer();
        // This event fires on finger release after a finger press and after the system 
        // voice command "Select" has been processed.For controllers, this event fires when 
        // the primary button is released after it was pressed.
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors if exists.
            if (FocusedObject != null)
            {
                // Calls the method named methodName on every MonoBehaviour in this game 
                // object and on every ancestor of the behaviour.
                FocusedObject.SendMessageUpwards("OnSelect");
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused _this frame_.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;
        RaycastHit hitInfo;  // Structure used to get information back from a raycast.
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            // Cancels any pending gesture events. Additionally this will call StopCapturingGestures.
            recognizer.CancelGestures();

            recognizer.StartCapturingGestures();
        }
    }
}
