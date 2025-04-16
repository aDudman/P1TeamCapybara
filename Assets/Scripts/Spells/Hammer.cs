using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField, Tooltip("The duration of the hammer swing")]
    private float duration = 0.2f;

    [SerializeField, Tooltip("The delay before the hammer swing starts")]
    private float delay = 0.5f;

    [SerializeField, Tooltip("The angle of the hammer backswing")]
    private float angle = 120f;

    [SerializeField, Tooltip("The sound that will play when the hammer is swung.")]
    private AudioSource swingSound;

    private float delayProgress;
    private float swingProgress;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(-angle, 0, 0); // Rotate the hammer to the starting position
        swingProgress = 0f; // Initialize swing progress
        delayProgress = 0f; // Initialize delay progress
    }

    private void FixedUpdate()
    {
        if (delayProgress < delay)
        {
            delayProgress += Time.fixedDeltaTime;
        }
        else if (swingProgress < angle)
        {
            if (swingSound != null && !swingSound.isPlaying)
            {
                swingSound.Play();
                swingSound.time = 0.3f;
            }
            float frameProgress = angle * Time.fixedDeltaTime / duration;

            // Rotate the hammer around its local Z axis
            transform.Rotate(frameProgress, 0, 0);

            // Update swing progress
            swingProgress += frameProgress;

            if (swingProgress >= angle)
            {
                Destroy(gameObject, 0.2f); // Destroy the hammer after the swing is complete
            }
        }    
    }
}
