using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Environment.Interactables
{
    /// <summary>
    /// This object will be carried over the players head with no powers needed.
    /// </summary>
    public class SmallObjectPickupable : MonoBehaviour, IPickupObject
    {

        [SerializeField, Tooltip("Set this to the max size of the object.")]
        private float smallObjectSize = 1;

        private float debugDrawBox { get; set; }
        private Vector3 placeLocation { get; set; }

        private BoxCollider boxCollider;
        private BoxCollider smallObjectBoxCollider
        {
            get
            {
                if (boxCollider is null)
                {
                    boxCollider = GetComponent<BoxCollider>();
                }
                return boxCollider;
            }
            set
            {
                boxCollider = value;
            }
        }


        // We just want the player to pick up the object for now.
        public bool CanPickupObject(GameObject agent)
        {
            return agent.CompareTag(Data.Globals.StaticTagStrings.PLAYER);
        }

        public virtual void PickupObject(GameObject agent)
        {
        }


        public bool CanPlaceObject(GameObject entity, Vector3 placedPosition)
        {
            // We don't currently want to place in anything.
            if (entity is not null)
            {
                return false;
            }

#if UNITY_EDITOR
            debugDrawBox = 1f;
            placeLocation = placedPosition;
#endif


            // If this collides with anything, then don't place.
            if (Physics.CheckBox(placedPosition, smallObjectBoxCollider.size / 2))
            {
                return false;
            }

            // If everything goes right, then place block.
            return true;
        }

        public void PlaceObject(GameObject entity, Vector3 placedPosition)
        {
            transform.position = placedPosition;
        }

        void OnDrawGizmos()
        {
            if (debugDrawBox > 0f)
            {
                debugDrawBox -= Time.deltaTime;
                Gizmos.DrawCube(placeLocation, smallObjectBoxCollider.size);
            }
        }

        public float PlaceDistanceFromEntity()
        {
            return smallObjectSize;
        }
    }

}
