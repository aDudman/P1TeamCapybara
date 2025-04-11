using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Toggleables
{
    public class TransparentBarrier : MonoBehaviour
    {

        private Collider barrierCollider { get; set; }

        void Start()
        {
            barrierCollider = GetComponent<Collider>();
        }

        /// <summary>
        /// Toggles the barrier on and off.
        /// </summary>
        /// <param name="state">True means the barrier is up.</param>
        public void SetBarrier(bool state = false)
        {

#if UNITY_EDITOR
            // The barrier collider wasn't set because Start wasn't run.
            barrierCollider = GetComponent<Collider>();

            Material localMaterial = GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().sharedMaterial);
            // Material localMaterial = GetComponent<Renderer>().material;
#endif
            barrierCollider.enabled = state;

            // Do visuals here and other stuff if needed.
            if (state)
            {
                // I just wanted some fancy tools for the barrier to debug it without running the game.  Now it just looks really messy.
#if UNITY_EDITOR
                localMaterial.SetColor("_Color", new Color(1f, 1f, 1f, .9f));
#else
                GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, .9f));
#endif
            }
            else
            {

#if UNITY_EDITOR
                localMaterial.SetColor("_Color", new Color(1f, 1f, 1f, .1f));
#else
                GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, .1f));
#endif

            }
        }

        // Buttons for the engine that'll assist in toggling the barrier state.
        #region Unity Buttons
#if UNITY_EDITOR
        [ContextMenu("Disable Barrier")]
        public void DisableBarrier()
        {
            SetBarrier(false);
        }

        [ContextMenu("Enable Barrier")]
        public void EnableBarrier()
        {
            SetBarrier(true);
        }
#endif
        #endregion

    }
}

