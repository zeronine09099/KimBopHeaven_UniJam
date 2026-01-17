using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class SimpleNumberText : MonoBehaviour
    {
        
        [SerializeField] private int number;
        [SerializeField] private TMPro.TextMeshProUGUI textComponent;
        
        
        [field:SerializeField]public string FormatString { get; set; } = "{0}";
        
        public int Number
        {
            get => number;
            set
            {
                number = value;
                UpdateText();
            }
        }

        private void Reset()
        {
            textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        }

        private void Awake()
        {
            if (textComponent == null)
                textComponent = GetComponent<TMPro.TextMeshProUGUI>();
            UpdateText();
        }
        
        private void UpdateText()
        {
            if (textComponent != null)
            {
                textComponent.text = string.Format(FormatString, number);
            }
        }

        private void OnValidate()
        {
            UpdateText();
        }

        public Tween CountTo(int start, int end, float duration)
        {
            number = start;
            UpdateText();
            return DOTween.To(() => number, x =>
            {
                number = x;
                UpdateText();
            }, end, duration);
        }
    }
}