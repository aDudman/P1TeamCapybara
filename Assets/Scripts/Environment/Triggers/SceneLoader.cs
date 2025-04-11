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
    }
}

