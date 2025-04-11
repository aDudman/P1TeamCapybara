using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class PlaymodeCamera : MonoBehaviour
    {
        // So that anything can access the camera when they need to.
        public static PlaymodeCamera CAMERA { get; private set; }


        [Tooltip("Follows the Gameobject on the X plane when value is true.")]
        public bool FollowOnX;

        [Tooltip("Follows the Gameobject on the Z plane when value is true.")]
        public bool FollowOnZ;

        [Tooltip("The object that is being followed.")]
        public GameObject FollowObject;

        [Tooltip("The constant speed that the camera follows at.")]
        public float FollowSpeed;

        void Start()
        {
            // Assigning the camera.
            CAMERA = this;
        }

        void Update()
        {
            if (FollowObject is not null)
            {
                // This logic needs an additional piece to work correctly.  Like, a follow behind object that is the thing that stops and starts.
                // Otherwise the camera stops at inconsistent spacing.
                #region Smooth follow camera following logic
                // Smooth Camera Logic
                // Vector3 moveVector = FollowObject.transform.position - transform.position;
                // moveVector.y = 0f;

                // if (FollowOnX)
                // {
                //     moveVector.x = ReturnSmallest(Mathf.Sign(moveVector.x) * FollowSpeed, moveVector.x);
                // }
                // else
                // {
                //     moveVector.x = 0f;
                // }

                // if (FollowOnZ)
                // {
                //     moveVector.z = ReturnSmallest(Mathf.Sign(moveVector.z) * FollowSpeed, moveVector.z);
                // }
                // else
                // {
                //     moveVector.z = 0f;
                // }

                // // Smoothing out the camera
                // moveVector *= Time.deltaTime;

                // Finalize movement
                // transform.position += moveVector;
                #endregion

                // This logic can be a bit snappy when going in and out of the trigger zones.  Also requires the Update function rather than
                // FixedUpdate as the player moves on the Update function.
                #region On Player follow logic
                Vector3 moveTo = MainCharacter.MCAgent.MCAGENT.transform.position;
                moveTo.y = transform.position.y;

                if (!FollowOnX)
                {
                    moveTo.x = transform.position.x;
                }

                if (!FollowOnZ)
                {
                    moveTo.z = transform.position.z;
                }

                transform.position = moveTo;
                #endregion
            }
        }

        /// <summary>
        /// Returns the smallest of 2 numbers.
        /// </summary>
        /// <param name="num1">First number</param>
        /// <param name="num2">Second number</param>
        /// <returns>Smallest number</returns>
        private float ReturnSmallest(float num1, float num2)
        {
            if (Mathf.Abs(num1) > Mathf.Abs(num2))
            {
                return num2;
            }
            return num1;
        }
    }

}
