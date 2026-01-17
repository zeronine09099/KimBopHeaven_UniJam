using Common.Singleton;
using UnityEngine;

namespace UI
{
    public class FloatingUICanvas : Singleton<FloatingUICanvas>
    {
        protected override void AfterAwake()
        {
            
        }

        public static Vector2 GetCanvasPosition(Vector3 worldPosition)
        {
            Camera cam = Camera.main;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);
            return pos;
        }
    }
}