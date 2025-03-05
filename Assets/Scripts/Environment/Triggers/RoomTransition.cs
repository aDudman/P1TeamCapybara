using Cinemachine;
using Data;
using Data.Globals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Triggers
{
    public class RoomTransition : MonoBehaviour
    {
        [SerializeField, Tooltip("Virtual camera to use in this room")]
        private CinemachineVirtualCamera roomCamera;

        [SerializeField, Tooltip("Position data for this room")]
        private RoomData roomData;

        private RoomManager roomManager;

        [SerializeField, Tooltip("The northern door")]
        private Door upDoor;

        [SerializeField, Tooltip("The southern door")]
        private Door downDoor;

        [SerializeField, Tooltip("The eastern door")]
        private Door rightDoor;

        [SerializeField, Tooltip("The western door")]
        private Door leftDoor;

        public event System.Action OnRoomEnter;
        public event System.Action OnRoomExit;

        private void Start()
        {
            roomManager = FindObjectOfType<RoomManager>();
            if (roomData == null)
            {
                Debug.LogError($"Room data is not set for {gameObject.scene.name}.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER))
            {
                roomCamera.enabled = true;
                roomCamera.Follow = other.transform;

                if (OnRoomEnter != null)
                {
                    OnRoomEnter.Invoke();
                }

                AdjacentRooms adjacentRooms = roomManager.GetAdjacentRooms(roomData.position);
                List<string> adjacentScenes = new List<string>();

                if (adjacentRooms.current == null)
                {
                    Debug.LogError($"Room at {roomData.position} is not in the room manager.");
                    return;
                }
                else
                {
                    adjacentScenes.Add(adjacentRooms.current);
                }

                if (adjacentRooms.up == null)
                {
                    upDoor.CloseDoor();
                }
                else
                {
                    adjacentScenes.Add(adjacentRooms.up);
                }

                if (adjacentRooms.down == null)
                {
                    downDoor.CloseDoor();
                }
                else
                {
                    adjacentScenes.Add(adjacentRooms.down);
                }

                if (adjacentRooms.right == null)
                {
                    rightDoor.CloseDoor();
                }
                else
                {
                    adjacentScenes.Add(adjacentRooms.right);
                }

                if (adjacentRooms.left == null)
                {
                    leftDoor.CloseDoor();
                }
                else
                {
                    adjacentScenes.Add(adjacentRooms.left);
                }

                SceneLoader.loader.LoadScenes(adjacentScenes);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER))
            {
                roomCamera.enabled = false;
                roomCamera.Follow = null;

                if (OnRoomExit != null)
                {
                    OnRoomExit.Invoke();
                }
            }
        }

        public void SetRoomData(RoomData roomData)
        {
            this.roomData = roomData;
        }
    }
}

