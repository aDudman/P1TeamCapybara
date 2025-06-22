using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField, Tooltip("The duration of the sword swing")]
    private float duration = 1f;

    [SerializeField, Tooltip("The angle of the sword swing")]
    private float angle = 90f;

    private float swingProgress;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, duration); // Destroy the sword after 2 seconds

        transform.Rotate(0, -angle/2, 0); // Rotate the sword to the starting position
        swingProgress = 0f; // Initialize swing progress
    }

    private void FixedUpdate()
    {
        float frameProgress = angle * Time.fixedDeltaTime / duration;

        // Rotate the sword around its local Y axis
        transform.Rotate(0, frameProgress, 0);

        // Update swing progress
        swingProgress += frameProgress;

        if (swingProgress >= angle)
        {
            Destroy(gameObject); // Destroy the sword after the swing is complete
        }


    }
}
