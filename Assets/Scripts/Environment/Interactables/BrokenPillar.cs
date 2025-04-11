using System;
using System.Collections;
using System.Collections.Generic;
using Environment.Toggleables;
using UnityEngine;


namespace Environment.Interactables
{
    public class BrokenPillar : AShoveable
    {
        [SerializeField, Tooltip("Direction you must shove the pillar in order for it to collapse.")]
        private Vector3 pushInDirection;

        [SerializeField, Tooltip("The amount of time that is required for the pillar to be pushed over.")]
        private float requiredShoveTime;
        [SerializeField, Tooltip("The time that the player needs to not push the pillar for the time to push to reset.")]
        private float resetRequiredTime = 1f;

        [SerializeField, Tooltip("The barrier that we're disabling.")]
        private TransparentBarrier barrier;


        // The magic structure class should go here so that it can be disabled when the pillar is knocked over.

        // How long the player has been shoving the pillar for.
        private float currentShoveTime = 0f;

        // The duration that the player has not been shoving the pillar for.
        private float currentResetTime = 0f;
        // Whether the pillar can be pushed.
        private bool canShove { get; set; }

        private Animator pillarAnimations { get; set; }

        void Start()
        {
            pillarAnimations = GetComponent<Animator>();
        }

        void Update()
        {
            if (currentShoveTime > 0f && currentResetTime < resetRequiredTime)
            {
                currentResetTime += Time.deltaTime;
                if (currentResetTime > resetRequiredTime)
                {
                    currentShoveTime = 0f;
                }
            }
        }


        public void EnablePushing()
        {
            canShove = true;
            pillarAnimations.SetBool("Unstable", true);
        }



        public override Vector3 Shoving(Vector3 direction)
        {
            if (canShove)
            {
                currentShoveTime += Time.fixedDeltaTime;
                currentResetTime = 0f;
                if (currentShoveTime > requiredShoveTime)
                {
                    PushPillar();
                }
            }
            return Vector3.zero;
        }

        private void PushPillar()
        {
            canShove = false;
            pillarAnimations.SetTrigger("Falling");
            barrier?.SetBarrier(false);
        }


    }
}

