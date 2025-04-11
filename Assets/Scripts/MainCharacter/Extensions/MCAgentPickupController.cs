using System;
using Environment.Interactables;
using UnityEngine;
using MainCharacter;

namespace MainCharacter.Extensions
{
    public class MCAgentPickupController : MonoBehaviour
    {
        [SerializeField, Tooltip("The animation for the players picking up and placing.")]
        private Animator animationBehaviour;

        [SerializeField, Tooltip("The distance from the player that they'll attempt to place the object.")]
        [Range(.01f, 3f)]
        // 1 is the default distance.
        private float placeDistance = 1;

        [SerializeField, Tooltip("Raycast for detecting objects that can be picked up.")]
        private Data.SRaycast sPickupRayCast;

        [SerializeField, Tooltip("This is the main character link.  It should be the parent object.")]
        private MCAgent mCAgent;

        // This records the last object hit with the raycast when placing.
        private GameObject lastHitObject { get; set; }

        // Keeps record of what the player is holding.
        public GameObject HeldObject { get; private set; }


        /// <summary>
        /// Trigger the event when player attempts to pickup an object and pass whether they succeed at it.
        /// </summary>
        public event Action<bool, GameObject> PlayerPickUpAttempt;

        /// <summary>
        /// Triggers when the player attempts to place the small object.
        /// </summary>
        public event Action<bool, GameObject> PlayerPlaceAttempt;

        /// <summary>
        /// Only runs on a success.  Sends the small object as a parameter.
        /// </summary>
        public event Action<GameObject> PlayerPickedUpObject;

        /// <summary>
        /// Only runs on a success.  Sends the small object as a parameter.
        /// </summary>
        public event Action<GameObject> PlayerPlacedObject;

        // This is a bool within the animator
        private const string animatorParameterObjectHeld = "ObjectHeld";
        // This is a trigger in the animator
        private const string animatorParameterFailedTask = "FailedTask";

        void Start()
        {
            PlayerPickUpAttempt += PlayPickupAnimation;
            PlayerPlaceAttempt += PlayPlaceAnimation;
        }

        void OnDestroy()
        {
            PlayerPickUpAttempt -= PlayPickupAnimation;
            PlayerPlaceAttempt -= PlayPlaceAnimation;
        }

        /// <summary>
        /// Receives the instructions to interact with a small object if possible.
        /// </summary>
        public void InteractionAttempted()
        {
            if (HeldObject is null)
            {
                PickupObject();
                return;
            }
            PlaceSmallObject();
        }


        /// <summary>
        /// This runs the picked up animation logic.
        /// </summary>
        /// <param name="pickedUp">A success is true.</param>
        /// <param name="passedObject">Not used currently within the animation.</param>
        private void PlayPickupAnimation(bool pickedUp, GameObject passedObject)
        {
            if (!pickedUp)
            {
                animationBehaviour.SetTrigger(animatorParameterFailedTask);
            }
            animationBehaviour.SetBool(animatorParameterObjectHeld, pickedUp);
        }


        /// <summary>
        /// This runs the place animation logic.
        /// </summary>
        /// <param name="pickedUp">A success is true.</param>
        /// <param name="passedObject">Not used currently within the animation.</param>
        private void PlayPlaceAnimation(bool placed, GameObject passedObject)
        {
            if (!placed)
            {
                animationBehaviour.SetTrigger(animatorParameterFailedTask);
            }
            animationBehaviour.SetBool(animatorParameterObjectHeld, !placed);
        }

        /// <summary>
        /// Detects when an object is a small pickup and attempts to pick it up.
        /// </summary>
        // The logic found here might be useful for other things that pick up objects, but as no such entities exist yet I'll leave it here for now.
        private void PickupObject()
        {
            RaycastHit hitInfo;
            if (Data.SRaycast.CastRayUsingSRaycast(sPickupRayCast, mCAgent.transform.position, mCAgent.transform, out hitInfo, true))
            {
                if (hitInfo.transform.CompareTag(Data.Globals.StaticTagStrings.SMALL_BLOCK))
                {
                    SmallObjectPickupable smallObjectPickupable = hitInfo.transform.gameObject.GetComponent<SmallObjectPickupable>();
                    if (smallObjectPickupable is null)
                    {
                        Debug.LogErrorFormat("${0} does not contain the Small Object Pickupable script", hitInfo.transform.name);
                        return;
                    }
                    if (!smallObjectPickupable.CanPickupObject(mCAgent.gameObject))
                    {
                        PlayerPickUpAttempt?.Invoke(false, smallObjectPickupable.gameObject);
                        return;
                    }

                    HeldObject = smallObjectPickupable.gameObject;
                    HeldObject.transform.SetParent(transform, true);
                    HeldObject.transform.localPosition = Vector3.zero;
                    PlayerPickUpAttempt?.Invoke(true, smallObjectPickupable.gameObject);
                    return;
                }
            }
        }

        /// <summary>
        /// Attempts to place the object that the main character is carrying.
        /// </summary>
        private void PlaceSmallObject()
        {
            IPickupObject pickedUpObjectComponent = HeldObject.GetComponent<IPickupObject>();

            if (pickedUpObjectComponent is null)
            {
                return;
            }

            RaycastHit hitInfo;
            Data.SRaycast.CastRayUsingSRaycast(sPickupRayCast, mCAgent.transform.position, mCAgent.transform, out hitInfo);

            // transform will be null if it hits nothing.  
            lastHitObject = hitInfo.transform?.gameObject;


            if (pickedUpObjectComponent.CanPlaceObject(lastHitObject, GetPlaceLocation()))
            {
                PlayerPlaceAttempt?.Invoke(true, HeldObject.gameObject);
                return;
            }

            PlayerPlaceAttempt?.Invoke(false, HeldObject.gameObject);
            return;
        }


        /// <summary>
        /// Little quick function for constant placing distance.
        /// </summary>
        /// <returns>The location the small object will be placed.</returns>
        private Vector3 GetPlaceLocation()
        {
            return mCAgent.transform.position + mCAgent.transform.forward * (placeDistance + HeldObject.GetComponent<IPickupObject>().PlaceDistanceFromEntity());
        }


        // Unity's animator events can't take a bool value.  0 for false and 1 for true.
        public void PickupObjectCompleted(int taskSucceed)
        {
            if (taskSucceed == 1)
            {
                PlayerPickedUpObject?.Invoke(HeldObject);
                HeldObject.GetComponent<IPickupObject>().PickupObject(mCAgent.gameObject);

            }
        }

        // Unity's animator events can't take a bool value.  0 for false and 1 for true.
        public void PlacedObjectCompleted(int taskSucceed)
        {
            if (taskSucceed == 1)
            {
                PlayerPlacedObject?.Invoke(HeldObject);
                HeldObject.GetComponent<IPickupObject>().PlaceObject(lastHitObject, GetPlaceLocation());
                HeldObject.transform.SetParent(null);
                HeldObject = null;
            }
        }
    }
}

