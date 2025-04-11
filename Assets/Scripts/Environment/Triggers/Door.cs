using Data.Globals;
using MainCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Triggers
{
    public class Door : MonoBehaviour
    {
        [SerializeField, Tooltip("When the trigger is entered, the player will move here")]
        private Transform Destination;

        [SerializeField, Tooltip("The trigger that will move the player")]
        private Collider transitionTrigger;

        [SerializeField, Tooltip("A model which blocks the door if it is closed")]
        private Collider closedDoorCollider;

        private bool doorOpen;
        public bool DoorOpen
        {
            get => doorOpen;
            set => SetDoor(value);
        }

        // Start is called before the first frame update
        void Start()
        {
            SetDoor(transitionTrigger.enabled);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void SetDoor(bool state)
        {
            if (state)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        [ContextMenu("Open Door")]
        public void OpenDoor()
        {
            doorOpen = true;
            closedDoorCollider.enabled = false;
            Material localMaterial = closedDoorCollider.GetComponent<Renderer>().material = new Material(closedDoorCollider.GetComponent<Renderer>().sharedMaterial);
            localMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
            transitionTrigger.enabled = true;
        }

        [ContextMenu("Close Door")]
        public void CloseDoor()
        {
            doorOpen = false;
            closedDoorCollider.enabled = true;
            Material localMaterial = closedDoorCollider.GetComponent<Renderer>().material = new Material(closedDoorCollider.GetComponent<Renderer>().sharedMaterial);
            localMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            transitionTrigger.enabled = false;
        }

        /// <summary>
        /// When a player enters a door transition, they will be forced to the next room
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER))
            {
                other.GetComponent<MCAgent>().ForceMovement(Destination.position);
            }
        }
    }
}
