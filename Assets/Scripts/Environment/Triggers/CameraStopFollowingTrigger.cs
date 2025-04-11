using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Environment.Triggers
{
    // Do not overlap triggers.  This will cause problems.
    [RequireComponent(typeof(BoxCollider))]
    public class CameraStopFollowingTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("Setting true will cause the camera to stop following on the X play when the player enters.")]
        private bool stopFollowingOnX;

        [SerializeField, Tooltip("Setting true will cause the camera to stop following on the Z play when the player enters.")]
        private bool stopFollowingOnZ;

        // This code will execute when the player enters the zone
        void OnTriggerEnter(Collider collider)
        {
            #region Player entering
            if (collider.CompareTag(Data.Globals.StaticTagStrings.PLAYER))
            {
                if (stopFollowingOnX)
                {
                    Camera.PlaymodeCamera.CAMERA.FollowOnX = false;
                }

                if (stopFollowingOnZ)
                {
                    Camera.PlaymodeCamera.CAMERA.FollowOnZ = false;
                }
            }
            #endregion
        }

        // This code will execute when the player leaves the zone
        void OnTriggerExit(Collider collider)
        {

            #region Player leaving
            if (collider.CompareTag(Data.Globals.StaticTagStrings.PLAYER))
            {
                if (stopFollowingOnX)
                {
                    Camera.PlaymodeCamera.CAMERA.FollowOnX = true;
                }

                if (stopFollowingOnZ)
                {
                    Camera.PlaymodeCamera.CAMERA.FollowOnZ = true;
                }
            }
            #endregion

        }

        // Run this only in the editor.  This is extremely likely to be put into it's own component as many triggers might want this functionality.
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            BoxCollider triggerBox = GetComponent<BoxCollider>();
            if (triggerBox is not null)
            {

                Gizmos.color = new Color
                (
                    stopFollowingOnX ? 1.0f : 0,
                    0f,
                    stopFollowingOnZ ? 1.0f : 0
                );
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(triggerBox.center, triggerBox.size);
            }
        }
#endif
    }
}

