using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Interactables
{

    public class AShoveable : MonoBehaviour
    {
        // The rigidbody that'll be moved.
        private Rigidbody moveableRigidbody { get; set; }

        // The amount of frames needed for the velocity to be cancelled out.
        protected int shoveFrameCount { get; set; }

        [SerializeField, Tooltip("A higher resistance mod will make the object move slower compared to the player.")]
        [Range(.01f, 1000f)]
        private float ResistanceMod;

        protected virtual void Start()
        {
            moveableRigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            // Cancelling out the velocity after x frames.
            if (shoveFrameCount > 0)
            {
                shoveFrameCount--;
                if (shoveFrameCount == 0)
                {
                    moveableRigidbody.velocity = Vector3.zero;
                }

            }
        }

        // This will be called in the Update when the player moves
        public virtual Vector3 Shoving(Vector3 direction)
        {
            // The amount of frames the velocity will be kept at.
            shoveFrameCount = 2;

            Vector3 moveWithResistance = direction / ResistanceMod;
            moveableRigidbody.velocity = moveWithResistance;
            return moveWithResistance;

        }
    }
}

