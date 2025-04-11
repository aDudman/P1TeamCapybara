using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    // Used to store raycast data to display in inspector.
    [Serializable]
    public struct SRaycast
    {

        [SerializeField, Tooltip("Offset from the raycast starting point which will be the object that is starting the cast.")]
        private Vector3 localOffset;

        [SerializeField, Tooltip("Set to true if you wish to use the objects local direction instead of using world direction.")]
        private bool isDirectionLocal;

        [SerializeField, Tooltip("The direction the ray will be cast in.  The direction does not contain distance.")]
        private Vector3 direction;

        [Tooltip("The max units the ray will go to.")]
        public float MaxDistance;

        [Tooltip("Layers that the ray will be hitting.")]
        public LayerMask LayerMask;

        /// <summary>
        /// Used to get the direction of the ray.  Set it to world or local in the inspector.
        /// </summary>
        /// <param name="rayTransform">The Transform that the ray originates from.</param>
        /// <returns>Returns the direction of the ray.</returns>
        public Vector3 Direction(Transform rayTransform)
        {
            if (isDirectionLocal)
            {
                return rayTransform.TransformDirection(direction);
            }
            return direction;
        }

        /// <summary>
        /// Gets the origin point at which the ray comes from with a local offset.
        /// </summary>
        /// <param name="startPoint">Set this to the origin point of the ray transform.</param>
        /// <returns>The origin point plus the local offset.</returns>
        public Vector3 Origin(Vector3 startPoint)
        {
            return localOffset + startPoint;
        }

        /// <summary>
        /// Uses the SRaycast Data type uniformally when it's needed to be used.
        /// </summary>
        /// <param name="rayData">The struct that contains the data.</param>
        /// <param name="originUsed">Where the ray originates from.</param>
        /// <param name="direction">Whether world or local transform.</param>
        /// <param name="rayHitData">The hit data if there is any.</param>
        /// <param name="drawRay">Will show a debug ray.</param>
        /// <returns>Returns true if the ray hits something.</returns>
        public static bool CastRayUsingSRaycast(SRaycast rayData, Vector3 originUsed, Transform baseTransform, out RaycastHit rayHitData, bool drawRay = false)
        {
            // I'm not sure if the debug draw ray shows up in the build, but let's just be careful.
#if UNITY_EDITOR
            if (drawRay)
            {
                Debug.DrawRay(rayData.Origin(originUsed), rayData.Direction(baseTransform), Color.red, .5f, false);
            }
#endif
            return Physics.Raycast(rayData.Origin(originUsed), rayData.Direction(baseTransform), out rayHitData, rayData.MaxDistance, rayData.LayerMask);
        }
    }

}
