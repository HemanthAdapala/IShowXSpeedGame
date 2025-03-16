using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugins
{
    public static class SceneLoader
    {
        public static event Action OnSceneLoadStarted;   // Fires when a scene starts loading
        public static event Action OnSceneLoadCompleted; // Fires when a scene is fully loaded

        /// <summary>
        /// Loads a scene by name.
        /// </summary>
        public static void LoadScene(string sceneName, bool additive = false)
        {
            OnSceneLoadStarted?.Invoke();
        
            if (additive)
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)!.completed += _ => OnSceneLoadCompleted?.Invoke();
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName)!.completed += _ => OnSceneLoadCompleted?.Invoke();
            }
        }

        /// <summary>
        /// Loads a scene by index.
        /// </summary>
        public static void LoadScene(int sceneIndex, bool additive = false)
        {
            OnSceneLoadStarted?.Invoke();
        
            if (additive)
            {
                SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive)!.completed += _ => OnSceneLoadCompleted?.Invoke();
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneIndex)!.completed += _ => OnSceneLoadCompleted?.Invoke();
            }
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        public static void ReloadCurrentScene()
        {
            OnSceneLoadStarted?.Invoke();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex)!.completed += _ => OnSceneLoadCompleted?.Invoke();
        }

        /// <summary>
        /// Loads the next scene in the build index order.
        /// </summary>
        public static void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("SceneLoader: No next scene found. Already at the last scene.");
            }
        }

        /// <summary>
        /// Unloads a scene by name.
        /// </summary>
        public static void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// Unloads a scene by index.
        /// </summary>
        public static void UnloadScene(int sceneIndex)
        {
            SceneManager.UnloadSceneAsync(sceneIndex);
        }
    }
}
