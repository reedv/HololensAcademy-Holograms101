using UnityEngine;

public class SphereSounds : MonoBehaviour
{
    // A representation of audio sources in 3D.
    AudioSource audioSource = null;
    // Containers for audio data.
    AudioClip impactClip = null;
    AudioClip rollingClip = null;

    bool rolling = false;

    void Start()
    {
        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 20f;

        // Load the Sphere sounds from the Assets/Resources/ folder
        impactClip = Resources.Load<AudioClip>("Impact");
        rollingClip = Resources.Load<AudioClip>("Rolling");
    }

    // Occurs when this object starts colliding with another object.
    // OnCollisionEnter is called when this collider/rigidbody has begun 
    // touching another rigidbody/collider.
    void OnCollisionEnter(Collision collision)
    {
        // Play an impact sound if the sphere impacts strongly enough.
        if (collision.relativeVelocity.magnitude >= 0.1f)
        {
            audioSource.clip = impactClip;  // set default audio clip for source
            audioSource.Play();
        }
    }

    // Occurs _each frame_ that this object _continues_ to collide with another object
    void OnCollisionStay(Collision collision)
    {
        Rigidbody rigid = this.gameObject.GetComponent<Rigidbody>();

        // Play a rolling sound if the sphere is rolling fast enough.
        if (!rolling && rigid.velocity.magnitude >= 0.01f)
        {
            rolling = true;
            audioSource.clip = rollingClip;
            audioSource.Play();
        }
        // Stop the rolling sound if rolling slows down.
        else if (rolling && rigid.velocity.magnitude < 0.01f)
        {
            rolling = false;
            audioSource.Stop();
        }
    }

    // Occurs when this object stops colliding with another object
    // OnCollisionExit is called when this collider/rigidbody has stopped 
    // touching another rigidbody/collider.
    void OnCollisionExit(Collision collision)
    {
        // Stop the rolling sound if the object falls off and stops colliding.
        if (rolling)
        {
            rolling = false;
            audioSource.Stop();
        }
    }
}
