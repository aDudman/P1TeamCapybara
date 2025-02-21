using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine;
using Data;
using UnityEditor;
using Data.Globals;
using System;
using UnityEditor.SceneManagement;
using Environment.Triggers;

namespace SceneTemplate
{
    public class BasicRoomPipeline : ISceneTemplatePipeline
    {
        private Scene createdScene;

        public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
        {
            return true;
        }

        public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
        {
            RoomCoordinateWindow.ShowWindow((coordinates, name) =>
            {
                HandleAfterTemplateInstantiation(sceneTemplateAsset, name, coordinates);
            });
        }

        private void HandleAfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, string sceneName, Vector2Int coordinates)
        {
            // Create RoomData
            RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
            roomData.sceneName = sceneName;
            roomData.position = coordinates;

            // Save RoomData asset
            string path = $"Assets/Scripts/Data/Rooms/{sceneName}_RoomData.asset";
            AssetDatabase.CreateAsset(roomData, path);
            AssetDatabase.SaveAssets();

            // Add RoomData to RoomManager's master list
            RoomManager roomManager = GameObject.FindObjectOfType<RoomManager>();
            if (roomManager != null)
            {
                roomManager.AddRoom(roomData);
            }
            else
            {
                Debug.LogError("RoomManager not found in scene.");
            }

            if (createdScene != null)
            {
                createdScene.name = sceneName;
                GameObject room = createdScene.GetRootGameObjects()[0];

                if (room != null)
                {
                    // Move Room game object to coordinates
                    room.transform.position = new Vector3(coordinates.x * RoomDefaults.roomX, 0, coordinates.y * RoomDefaults.roomY);

                    RoomTransition roomTransition = room.GetComponentInChildren<RoomTransition>();
                    if (roomTransition != null)
                    {
                        roomTransition.SetRoomData(roomData);
                        EditorUtility.SetDirty(roomTransition); // Mark the RoomTransition object as dirty
                    }
                    else
                    {
                        Debug.LogError($"RoomTransition component not found in {sceneName}.");
                    }
                }
                else
                {
                    Debug.LogError($"Room game object not found in {sceneName}.");
                }

                EditorSceneManager.SaveScene(createdScene, $"Assets/Scenes/StartDungeon/{sceneName}.unity");
            }
        }

        public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
        {
            createdScene = scene;
        }
    }
}
