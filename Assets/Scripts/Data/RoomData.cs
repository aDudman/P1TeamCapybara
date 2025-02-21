using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
    public class RoomData : ScriptableObject
    {
        public string sceneName;
        public Vector2Int position; // Assuming a 2D grid for simplicity
    }
}