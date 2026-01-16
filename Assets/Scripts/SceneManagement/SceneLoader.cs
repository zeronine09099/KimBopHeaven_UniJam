using System;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class SceneLoader
    {
        private static string firstLoadedSceneName = null;

        /// <summary>
        /// 씬이 처음 로드될 때 한 번만 호출되는 이벤트
        /// </summary>
        public static event Action<Scene, LoadSceneMode> OnSceneLoadedOnce;
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            firstLoadedSceneName = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoadedOnce?.Invoke(scene, mode);
            OnSceneLoadedOnce = null;
        }
        public static string GetFirstLoadedSceneName()
        {
            return firstLoadedSceneName;
        }
        
        public static bool IsStartedFromScene(Scenes sceneEnum)
        {
            return firstLoadedSceneName == SceneReference.Find(sceneEnum).SceneName;
        }
        
        public static bool IsInScene(Scenes sceneEnum)
        {
            return SceneManager.GetActiveScene().name == SceneReference.Find(sceneEnum).SceneName;
        }
        
        public static string GetSceneName(Scenes sceneEnum)
        {
            return SceneReference.Find(sceneEnum).SceneName;
        }
        
        public static void LoadSceneLocal(Scenes sceneEnum, bool additive = false)
        {
            LoadSceneLocal(SceneReference.Find(sceneEnum).SceneName, additive);
        }
        public static void LoadSceneLocal(string sceneName, bool additive = false)
        {
            var loadMode = additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single;
           SceneManager.LoadScene(sceneName, loadMode);
        }
        
        public static void UnloadSceneLocal(Scenes sceneEnum)
        {
            UnloadSceneLocal(SceneReference.Find(sceneEnum).SceneName);
        }
        public static void UnloadSceneLocal(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
        
        
        
            
    }
}