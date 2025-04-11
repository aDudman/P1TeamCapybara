using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Interactables
{
    /// <summary>
    /// Used when an object can be picked up in the scene.  This also includes other interactions that involve similar logic.
    /// </summary>
    public interface IPickupObject
    {
        /// <summary>
        /// Informs the agent on whether or not they are able to pick up the object.
        /// It might be a good idea to add in parameters other than the gameobject. 
        /// </summary>
        /// <param name="agent">The agent that is attempting to pick up the object.</param>
        /// <returns>Returns true if the object can be picked up by the agent.</returns>
        bool CanPickupObject(GameObject agent);

        /// <summary>
        /// This sends instructions for how the object reacts to being picked up.
        /// </summary>
        /// <param name="agent">The agent that is attempting to pick up the object.</param>
        void PickupObject(GameObject agent);


        /// <summary>
        /// Call this to confirm that the placement is a valid place to put the object.
        /// This might need a vector for where to place the object.
        /// </summary>
        /// <param name="entity">The thing that the object is being placed into.</param>
        /// <param name="placedPosition">The position that the object is being placed.</param>
        /// <returns>Returns true if the placement is valid.</returns>
        bool CanPlaceObject(GameObject entity, Vector3 placedPosition);

        /// <summary>
        /// Sends instructions for how the object should be placed in the scene.
        /// </summary>
        /// <param name="placedPosition">The position that the object is being placed.</param>
        /// <param name="entity">The position that the object is being placed.</param>
        void PlaceObject(GameObject entity, Vector3 placedPosition);

        float PlaceDistanceFromEntity();

    }

    public enum PickupState
    {
        PickedUp,
        Placed
    }
}
