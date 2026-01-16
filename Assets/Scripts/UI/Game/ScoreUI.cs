using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Slider scoreFillSlider;
        [SerializeField] private Slider scoreTempFillSlider;

        [Header("Settings")] 
        [SerializeField] private Ease fillEase = Ease.InQuad;
        [SerializeField] private float fillDuration = 0.5f;

        [Header("Current Values")] 
        [SerializeField] private float currentValue = 0;
        [SerializeField] private float tempValue = 0;
        [SerializeField] private float maxValue = 100f;


        public float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = value;
                UpdateUI();
            }
        }
        
        public float TempValue
        {
            get => tempValue;
            set
            {
                tempValue = Mathf.Min(value, currentValue);
                UpdateUI();
            }
        }
        
        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                UpdateUI();
            }
        }
        
        private void Awake()
        {
            
        }

        public void UpdateUI(float newValue, float newTempValue, float newMaxValue)
        {
            currentValue = newValue;
            tempValue = Mathf.Min(newTempValue, newValue);
            maxValue = newMaxValue;
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (DOTween.IsTweening(scoreFillSlider) || DOTween.IsTweening(scoreTempFillSlider))
            {
                DOTween.Complete(scoreFillSlider);
                DOTween.Complete(scoreTempFillSlider);
            }
            
            if (maxValue <= 0)
            {
                scoreFillSlider.DOValue(0, fillDuration).SetEase(fillEase);
                scoreTempFillSlider.DOValue(0, fillDuration).SetEase(fillEase);
                return;
            }
            
            tempValue = Mathf.Min(tempValue, currentValue);
            
            float targetFill = Mathf.Clamp01(currentValue / maxValue);
            float targetTempFill = Mathf.Clamp01(tempValue / maxValue);
            
            scoreFillSlider.DOValue(targetFill, fillDuration).SetEase(fillEase);
            scoreTempFillSlider.DOValue(targetTempFill, fillDuration).SetEase(fillEase);
        }
        
        private void UpdateUIImmediate()
        {
            if (maxValue <= 0)
            {
                scoreFillSlider.value = 0;
                scoreTempFillSlider.value = 0;
                return;
            }
            
            tempValue = Mathf.Min(tempValue, currentValue);
            
            scoreFillSlider.value = Mathf.Clamp01(currentValue / maxValue);
            scoreTempFillSlider.value = Mathf.Clamp01(tempValue / maxValue);
        }

        private void OnValidate()
        {
            UpdateUIImmediate();
        }
    }
}