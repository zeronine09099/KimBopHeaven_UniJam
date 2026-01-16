using System;
using Common.Singleton;
using Cysharp.Threading.Tasks;
using Database;
using Game;
using Machamy.Utils;
using SceneManagement;
using Sound;
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

        public int LoadedCount
        {
            get => _loadedCount;
            private set => _loadedCount = value;
        }

        private void Awake()
        {

        }

        private void Start()
        {
            Initialize().Forget();
        }
        
        private async UniTaskVoid Initialize()
        {
            LogEx.Log("Initializing Bootstrapper...");
            await InitializeCore();
            await InitializeScene();
            LogEx.Log("Bootstrapper initialization completed.");
        }

        private async UniTask InitializeCore()
        {
            void IncrementLoadedCount()
            {
                LoadedCount++;
                LogEx.Log($"[Bootstrapper] Loaded {LoadedCount}/{_totalToLoad}");
            }

            LoadedCount = 5; // DatabaseManager, GameManager, StageManager, IngredientManager, SoundManager
            LoadedCount = 0;
            
            // DatabaseManager 초기화
            DatabaseManager.Instance.Initialize();

            
            // 나머지 매니저들 병렬로 초기화
            await UniTask.WhenAll(
                GameManager.Instance.Init(IncrementLoadedCount),
                StageManager.Instance.Init(IncrementLoadedCount).ToUniTask(),
                IngredientManager.Instance.Init(IncrementLoadedCount).ToUniTask()
            );
            
            await UniTask.WaitUntil(() => SoundManager.Instance.IsInitialized);
            IncrementLoadedCount();
            
            await UniTask.WaitUntil(() => DatabaseManager.Instance.IsInitialized);
            IncrementLoadedCount();
            
            await UniTask.Yield();
        }

        private async UniTask InitializeScene()
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
            
            
            await UniTask.WaitUntil(() => _isCompleted);
        }
        
        
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _isCompleted = true;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}