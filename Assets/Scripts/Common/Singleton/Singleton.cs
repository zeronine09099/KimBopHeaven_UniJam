using UnityEngine;

namespace Common.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T _instance;
        
        protected virtual bool DontDestroyOnLoad => true;

        public static bool HasInstance => _instance != null;
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        private static void InitOnLoad()
        {
            UnityEditor.EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    _instance = null;
                }
            };
        }
#endif
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("Finding existing instance of " + typeof(T).Name);
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"No instance of {typeof(T).Name} found in the scene.");
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            Debug.Log($"Awake called for {typeof(T).Name}, Id: {GetInstanceID()}");

            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"Another instance of {typeof(T).Name} already exists. Destroying this instance.");
                Destroy(gameObject);
            }
            if (_instance == this)
            {
                if (DontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
                AfterAwake();
            }
        }
        
        protected abstract void AfterAwake();
    }
}