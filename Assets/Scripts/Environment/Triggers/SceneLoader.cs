using Data;
using Data.Globals;
using MainCharacter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Triggers
{
    public class SceneLoader : MonoBehaviour
    {
        private List<string> loadedScenes = new List<string>();

        [SerializeField, Tooltip("The room data for the starting room.")]
        private RoomData startingRoom;

        [SerializeField, Tooltip("The player object to be moved to the starting position.")]
        private MCAgent player;

        public static SceneLoader loader;

        // Start is called before the first frame update
        void Start()
        {
            if (loader == null)
            {
                loader = this;
            }
            else
            {
                Destroy(gameObject);
            }

            int sceneCount = SceneManager.loadedSceneCount;
            string mainScene = gameObject.scene.name;

            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != mainScene)
                {
                    loadedScenes.Add(scene.name);
                }
            }

            // If there are no rooms loaded, default to the starting room
            if (loadedScenes.Count == 0)
            {
                LoadStartingRoom();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Loads all scenes in a list, and unloads any other managed scenes
        /// </summary>
        /// <param name="newScenes">Names of scenes to load</param>
        public void LoadScenes(IEnumerable<string> newScenes)
        {
            // Probably could be optimized
            var unloadScenes = loadedScenes.Where(s => !newScenes.Contains(s)).ToList();
            var loadScenes = newScenes.Where(s => !loadedScenes.Contains(s)).ToList();

            foreach (var scene in loadScenes)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
                loadedScenes.Add(scene);
            }

            foreach (string scene in unloadScenes)
            {
                SceneManager.UnloadSceneAsync(scene);
                loadedScenes.Remove(scene);
            }
        }

        /// <summary>
        /// Loads the starting room scene if it is set, and moves the player to the starting position.
        /// </summary>
        void LoadStartingRoom()
        {
            if (startingRoom != null)
            {
                SceneManager.LoadSceneAsync(startingRoom.sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(startingRoom.sceneName);

                // Move the player to the starting position
                if (player != null)
                {
                    Debug.Log("Moving player to starting position.");
                    Debug.Log($"Player position: {player.transform.position}");
                    Debug.Log($"Starting room position: {startingRoom.position}");

                    // Calculate the target position
                    Vector3 targetPosition = new Vector3(
                        startingRoom.position.x * RoomDefaults.roomX,
                        player.transform.position.y,
                        startingRoom.position.y * RoomDefaults.roomY
                    );

                    // Use the CharacterController to move the player
                    CharacterController characterController = player.GetComponent<CharacterController>();
                    if (characterController != null)
                    {
                        Vector3 offset = targetPosition - player.transform.position;
                        characterController.Move(offset); // Use Move to adjust the position
                    }
                    else
                    {
                        // Fallback if no CharacterController is found
                        player.transform.position = targetPosition;
                    }

                    Debug.Log($"New player position: {player.transform.position}");
                }
                else
                {
                    Debug.LogError("Player object is not set.");
                }
            }
            else
            {
                Debug.LogError("Starting room is not set.");
            }
        }
    }
}

