using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class RoomDeletionWindow : EditorWindow
    {
        private List<RoomData> roomDataList;
        private RoomData selectedRoomData;
        private RoomManager roomManager;
        private int selectedIndex = 0;
        private string[] roomNames;

        [MenuItem("Tools/Delete Room")]
        public static void ShowWindow()
        {
            GetWindow<RoomDeletionWindow>("Delete Room");
        }

        private void OnEnable()
        {
            LoadRoomData();
            roomManager = FindObjectOfType<RoomManager>();
        }

        private void LoadRoomData()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomData", new[] { "Assets/Scripts/Data/Rooms" });
            roomDataList = guids.Select(guid => AssetDatabase.LoadAssetAtPath<RoomData>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
            roomNames = roomDataList.Select(room => room.sceneName).ToArray();
        }

        private void OnGUI()
        {
            GUILayout.Label("Select a Room to Delete", EditorStyles.boldLabel);

            if (roomDataList == null || roomDataList.Count == 0)
            {
                GUILayout.Label("No RoomData found in the specified folder.");
                return;
            }

            selectedIndex = EditorGUILayout.Popup("Room", selectedIndex, roomNames);
            selectedRoomData = roomDataList[selectedIndex];

            if (selectedRoomData != null)
            {
                GUILayout.Space(10);
                GUILayout.Label($"Selected Room: {selectedRoomData.sceneName}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Room"))
                {
                    DeleteSelectedRoom();
                }
            }
        }

        private void DeleteSelectedRoom()
        {
            if (selectedRoomData == null)
            {
                Debug.LogError("No room selected for deletion.");
                return;
            }

            // Remove the room from the RoomManager
            if (roomManager != null)
            {
                roomManager.RemoveRoom(selectedRoomData);
            }
            else
            {
                Debug.LogError("RoomManager not found in the scene.");
            }

            // Delete the scene
            string scenePath = $"Assets/Scenes/StartDungeon/{selectedRoomData.sceneName}.unity";
            if (AssetDatabase.DeleteAsset(scenePath))
            {
                Debug.Log($"Scene {selectedRoomData.sceneName} deleted successfully.");
            }
            else
            {
                Debug.LogError($"Failed to delete scene {selectedRoomData.sceneName}.");
            }

            // Unload the scene from the Hierarchy
            Scene scene = SceneManager.GetSceneByName(selectedRoomData.sceneName);
            if (scene.isLoaded)
            {
                EditorSceneManager.CloseScene(scene, true);
            }

            // Delete the RoomData asset
            string roomDataPath = AssetDatabase.GetAssetPath(selectedRoomData);
            if (AssetDatabase.DeleteAsset(roomDataPath))
            {
                Debug.Log($"RoomData {selectedRoomData.sceneName} deleted successfully.");
            }
            else
            {
                Debug.LogError($"Failed to delete RoomData {selectedRoomData.sceneName}.");
            }

            // Refresh the list
            LoadRoomData();
            selectedRoomData = null;
            selectedIndex = 0;
        }
    }
}