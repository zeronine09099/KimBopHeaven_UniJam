using System;
using TMPro;
using UnityEngine;

namespace UI.GameOver
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class PSElementUI : MonoBehaviour
    {
		public TextMeshProUGUI PSName;
        public TextMeshProUGUI PSValue;
        public CanvasGroup CanvasGroup;

        private void Reset()
        {
            PSName = transform.Find("Label").GetComponent<TextMeshProUGUI>();
            PSValue = transform.Find("Value").GetComponent<TextMeshProUGUI>();
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}