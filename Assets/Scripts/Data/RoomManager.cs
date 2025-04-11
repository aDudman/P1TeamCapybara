using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField]
        private List<RoomData> rooms;

        private Dictionary<Vector2Int, RoomData> roomDictionary;

        private void Awake()
        {
            roomDictionary = new Dictionary<Vector2Int, RoomData>();
            foreach (var room in rooms)
            {
                if (roomDictionary.ContainsKey(room.position))
                {
                    var existingRoom = roomDictionary[room.position];
                    Debug.LogWarning($"Room {existingRoom.sceneName} at {room.position} already exists. Skipping {room.sceneName}.");
                    continue;
                }
                roomDictionary[room.position] = room;
            }
        }

        public bool AddRoom(RoomData room)
        {
            if (!rooms.Contains(room))
            {
                rooms.Add(room);
                return true;
            }
            return false;
        }

        public bool RemoveRoom(RoomData room)
        {
            if (rooms.Contains(room))
            {
                rooms.Remove(room);
                return true;
            }
            return false;
        }

        public AdjacentRooms GetAdjacentRooms(Vector2Int currentPosition)
        {
            AdjacentRooms adjacentRooms = new AdjacentRooms();

            if (roomDictionary.TryGetValue(currentPosition, out RoomData currentRoom))
            {
                adjacentRooms.current = currentRoom.sceneName;
            }
            else
            {
                adjacentRooms.current = null;
            }

            if (roomDictionary.TryGetValue(currentPosition + Vector2Int.up, out RoomData adjacentRoom))
            {
                adjacentRooms.up = adjacentRoom.sceneName;
            }
            else
            {
                adjacentRooms.up = null;
            }

            if (roomDictionary.TryGetValue(currentPosition + Vector2Int.down, out adjacentRoom))
            {
                adjacentRooms.down = adjacentRoom.sceneName;
            }
            else
            {
                adjacentRooms.down = null;
            }

            if (roomDictionary.TryGetValue(currentPosition + Vector2Int.left, out adjacentRoom))
            {
                adjacentRooms.left = adjacentRoom.sceneName;
            }
            else
            {
                adjacentRooms.left = null;
            }

            if (roomDictionary.TryGetValue(currentPosition + Vector2Int.right, out adjacentRoom))
            {
                adjacentRooms.right = adjacentRoom.sceneName;
            }
            else
            {
                adjacentRooms.right = null;
            }

            Debug.Log($"current:{adjacentRooms.current}, up:{adjacentRooms.up}, down:{adjacentRooms.down}, right:{adjacentRooms.right}, left:{adjacentRooms.left}");

            return adjacentRooms;
        }
    }

    public struct AdjacentRooms
    {
        public string up;
        public string down;
        public string left;
        public string right;
        public string current;
    }
}
