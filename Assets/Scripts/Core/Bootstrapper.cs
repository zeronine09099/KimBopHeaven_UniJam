using System;
using System.Collections;
using System.Collections.Generic;
using Common.Singleton;
using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        #if UNITY_EDITOR
        public const string EditorStartScenePathKey = "Bootstrapper.EditorStartScenePath";
        public const string EditorStartSceneNameKey = "Bootstrapper.EditorStartSceneName";
        
        public static string ToLoadSceneNameInEditor => UnityEditor.EditorPrefs.GetString(EditorStartSceneNameKey, null);
        #endif
        // 완료 여부
        private static bool _isCompleted = false;
        
        public static bool IsCompleted => _isCompleted;
        


        private void Awake()
        {

        }

        private IEnumerator Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            #if UNITY_EDITOR
            Debug.Log($"[Bootstrapper] Editor mode: Loading scene '{ToLoadSceneNameInEditor}' from EditorPrefs.");
            if (!string.IsNullOrEmpty(ToLoadSceneNameInEditor))
            {
                SceneLoader.LoadSceneLocal(ToLoadSceneNameInEditor);
            }
            else
            {
                SceneLoader.LoadSceneLocal(Scenes.MainScene);
            }
            #else 
            SceneLoader.LoadSceneLocal(Scenes.MainTitle);
            #endif
            
            
            yield return new WaitUntil(() => _isCompleted);
        }
        
        
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _isCompleted = true;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}