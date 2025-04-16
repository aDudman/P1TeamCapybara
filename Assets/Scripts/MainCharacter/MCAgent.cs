using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Environment.Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainCharacter
{
    public class MCAgent : MonoBehaviour
    {
        // Used when referencing the player character
        public static MCAgent MCAGENT { get; private set; }

        [SerializeField, Tooltip("The speed that the player will move with no other modifiers.")]
        private float playerSpeed;

        [SerializeField, Tooltip("Drag the players character controller onto this field.")]
        private CharacterController characterController;


        [SerializeField, Tooltip("The data for the pushing raycast.")]
        private Data.SRaycast sPushRayCast;

        [SerializeField, Tooltip("Prefab to be instantiated when a spell is cast. Probably reworked later.")]
        private GameObject spell;

        private bool spellsEnabled;

        private bool inputDisabled;

        private Vector2 forcedMovementDestination;

        [SerializeField, Tooltip("The component that is able to handle picking up logic.")]
        private Extensions.MCAgentPickupController pickupController;

        [SerializeField, Tooltip("The animator for the player.")]
        private Animator animator;

        // This region only accepts inputs that the player will be using.  It will only pull from the global input system.
        // Set these on Start.
        #region Input Action
        // This action is directing the players movement.
        private InputAction movementAction { get; set; }

        // This is named as such so that it follows the documentation in the GDD.
        private InputAction actionAction { get; set; }
        #endregion




        void Start()
        {
            MCAGENT = this;
            inputDisabled = false;
            if (spell != null)
            {
                EnableSpells();
            }
            movementAction = InputSystem.actions.FindAction("Move");
            actionAction = InputSystem.actions.FindAction("Action");
        }

        void FixedUpdate()
        {
            // Collect the move data here
            Vector2 characterCompleteMoveV2 = Vector2.zero;

            if (inputDisabled)
            {
                characterCompleteMoveV2 = ForcedMovement();
            }
            else
            {
                characterCompleteMoveV2 = PlayerMovement();
            }

            Vector3 characterCompleteMoveV3 = fromVector2(characterCompleteMoveV2);

            // Using the sqrMagnitude because it avoids using sqrt which is a costly calculation
            // Use this section to activate anything that needs the players movement to occur.
            // Does not include external movement.
            if (characterCompleteMoveV3.sqrMagnitude > 0.00f)
            {
                // This will be snappy.  Can update it later to be more smooth.
                // Look at calculation might not be needed either.
                transform.LookAt(transform.position + characterCompleteMoveV3);

                ShoveObject(ref characterCompleteMoveV3);

                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }

            characterController.Move(characterCompleteMoveV3 * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!inputDisabled)
            {
                // Code block for picking up and dropping small objects in the scene.
                if (actionAction.WasPressedThisFrame())
                {
                    pickupController.InteractionAttempted();
                }

                CheckSpellCast();
            }
        }

        /// <summary>
        /// Logic for pushing blocks.  Currently only handles small blocks.
        /// </summary>
        private void ShoveObject(ref Vector3 direction)
        {
            RaycastHit hitInfo;
            if (SRaycast.CastRayUsingSRaycast(sPushRayCast, transform.position, transform, out hitInfo, true))
            {
                if (hitInfo.transform.CompareTag(Data.Globals.StaticTagStrings.SHOVEABLE))
                {
                    direction = hitInfo.transform.GetComponent<AShoveable>().Shoving(direction);
                }
            }
        }

        /// <summary>
        /// Logic for casting spells. Currently only casts Push
        /// </summary>
        private void CheckSpellCast()
        {
            var cast = Input.GetKeyDown(KeyCode.I);
            if (spellsEnabled && cast)
            {
                var spellEffect = Instantiate(spell, transform.position, transform.rotation);
                StartCoroutine(castDelay(1.0f));
            }
        }

        /// <summary>
        /// Will return a vector2 that indicates the direction of the players movement based on user input.
        /// </summary>
        /// <returns>Vector2.  Positive indicates right and up. Negative is left and down.  Zero is no movement</returns>
        private Vector2 PlayerMoveDirection()
        {
            // Will only display the direction of the input.  Will not have modifiers used in final move.
            Vector2 outputDirection = movementAction.ReadValue<Vector2>();
            return outputDirection;
        }

        /// <summary>
        /// This takes into account any modifiers applied to the players move direction.
        /// </summary>
        /// <returns>Move with modifiers.  Includes the time delta as well.</returns>
        private Vector2 PlayerMovement()
        {
            // Set to zero in case the re assignment doesn't happen later.
            Vector2 outMove = Vector2.zero;

            outMove = PlayerMoveDirection() * playerSpeed;

            return outMove;
        }

        /// <summary>
        /// Continues movement toward a destination, ending when close enough
        /// </summary>
        /// <returns></returns>
        private Vector2 ForcedMovement()
        {
            Vector2 outMove = Vector2.zero;
            Vector2 difference = (forcedMovementDestination - fromVector3(transform.position));
            if (difference.magnitude < playerSpeed * Time.deltaTime)
            {
                inputDisabled = false;
            }
            outMove = difference.normalized * playerSpeed;

            return outMove;
        }

        public void EnableSpells()
        {
            spellsEnabled = true;
        }

        /// <summary>
        /// Forces player to move to a position without allowing inputs
        /// </summary>
        /// <param name="destination"></param>
        public void ForceMovement(Vector3 destination)
        {
            // For now, simply ignore other triggers
            if (inputDisabled) return;

            forcedMovementDestination = fromVector3(destination);
            inputDisabled = true;
        }

        /// <summary>
        /// Coroutine to temporarily disable casting spells
        /// </summary>
        /// <param name="seconds">Number of seconds to wait</param>
        /// <returns></returns>
        private IEnumerator castDelay(float seconds)
        {
            spellsEnabled = false;
            yield return new WaitForSeconds(seconds);
            spellsEnabled = true;
        }

        /// <summary>
        /// Our Vector2 should use x and z, not x and y
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private Vector2 fromVector3(Vector3 from)
        {
            return new Vector2(from.x, from.z);
        }

        private Vector3 fromVector2(Vector2 from)
        {
            return new Vector3(from.x, 0, from.y);
        }
    }

}
