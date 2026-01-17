using System;
using Common.Singleton;
using Machamy.Attributes;
using UnityEngine;

namespace Core
{
    public class Letterbox : MonoBehaviour
    {
        
        
        public float targetWidth = 9.0f;
        public float targetHeight = 19.5f;

        private Camera _camera;
        private float _lastScreenWidth;
        private float _lastScreenHeight;

        void Start()
        {
            _camera = GetComponent<Camera>();
            UpdateAspectRatio();
        }

        void Update()
        {
            if (!Mathf.Approximately(Screen.width, _lastScreenWidth) || !Mathf.Approximately(Screen.height, _lastScreenHeight))
            {
                UpdateAspectRatio();
            }
        }

        void UpdateAspectRatio()
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;

            float targetAspect = targetWidth / targetHeight;
            float windowAspect = (float)Screen.width / (float)Screen.height;
            float scaleHeight = windowAspect / targetAspect;

            Rect rect = _camera.rect;

            if (scaleHeight < 1.0f) // 레터박스 (위아래)
            {
                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
            }
            else // 필러박스 (좌우)
            {
                float scaleWidth = 1.0f / scaleHeight;
                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;
            }

            _camera.rect = rect;
        }


    }
}