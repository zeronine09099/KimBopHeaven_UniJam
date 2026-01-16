using UnityEngine;

namespace Common.Extentions
{
    public static class GameobjectExtention
    {
        
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
        
        public static T GetOrAddComponentInChildren<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponentInChildren<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
        
        public static T GetOrAddComponentInParent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponentInParent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
        
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

    }
}