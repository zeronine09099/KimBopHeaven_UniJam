using System;
using System.Collections;
using System.Collections.Generic;
using Common.Singleton;
using Database;
using Game;
using Machamy.Utils;
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
        

        private int _totalToLoad = 0;
        private int _loadedCount = 0;
        public int TotalToLoad => _totalToLoad;
        public int LoadedCount => _loadedCount;

        private void Awake()
        {

        }

        private IEnumerator Start()
        {
            yield return InitializeCore();
            yield return InitializeScene();
        }

        private IEnumerator InitializeCore()
        {
            _totalToLoad = 4; // 매니저 수에 맞게 설정
            _loadedCount = 0;
            DatabaseManager.Instance.Initialize();
            
            void IncrementLoadedCount()
            {
                _loadedCount++;
                LogEx.Log($"[Bootstrapper] Loaded {_loadedCount}/{_totalToLoad}");
            }
            
            yield return GameManager.Instance.Init(IncrementLoadedCount);
            yield return StageManager.Instance.Init(IncrementLoadedCount);
            yield return IngredientManager.Instance.Init(IncrementLoadedCount);

            yield return new WaitWhile(() => DatabaseManager.Instance.IsInitialized);
            IncrementLoadedCount();
            
            yield return null;
        }

        private IEnumerator InitializeScene()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
#if UNITY_EDITOR
            LogEx.Log($"[Editor mode: Loading scene '{ToLoadSceneNameInEditor}' from EditorPrefs.");
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