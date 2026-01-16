using UnityEngine;

namespace Common.Singleton
{
    public abstract class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
    {
        protected static T _instance;

        protected virtual bool DontDestroyOnLoad => true;
        public static bool HasInstance => _instance != null;

        public static T Instance
        {
            get
            {
                // Lazy initialization
                if (_instance == null)
                {
                    Debug.Log("Finding existing instance of " + typeof(T).Name);
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("@"+typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
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