using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Slider scoreFillSlider;
        [SerializeField] private Slider scoreTempFillSlider;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI scoreText2;

        [Header("Settings")] 
        [SerializeField] private Ease tempFillEase = Ease.OutQuad;
        [SerializeField] private float tempFillDuration = 0.2f;
        [SerializeField] private Ease fillEase = Ease.OutQuad;
        [SerializeField] private float fillDuration = 0.5f;

        [Header("Current Values")] 
        [SerializeField] private float currentValue = 0;
        [SerializeField] private float tempValue = 0;
        [SerializeField] private float maxValue = 100f;

        private Tween _scoreTextTween;
        private int _displayScore;

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                // 값이 같으면 무시
                if (Mathf.Approximately(currentValue, value)) return;
                currentValue = value;
                if (currentValue > tempValue)
                {
                    // Current가 Temp보다 커지면 Temp도 같이 올려줌
                    tempValue = currentValue;
                    UpdateTempUI();
                }
                UpdateCurrentUI(); // Current만 갱신
            }
        }
        
        public float TempValue
        {
            get => tempValue;
            set
            {
                if (Mathf.Approximately(tempValue, value)) return;
                tempValue = value;
                UpdateTempUI(); // Temp만 갱신
            }
        }
        
        public float MaxValue
        {
            get => maxValue;
            set
            {
                if (Mathf.Approximately(maxValue, value)) return;
                maxValue = value;
                // Max가 바뀌면 비율이 달라지므로 둘 다 갱신
                UpdateCurrentUI();
                UpdateTempUI();
            }
        }
        
        private void Awake()
        {
            _displayScore = (int)currentValue;
        }

        // Current 관련 UI만 업데이트 (슬라이더 + 텍스트)
        private void UpdateCurrentUI()
        {
            if (maxValue <= 0)
            {
                scoreFillSlider.DOValue(0, fillDuration).SetEase(fillEase);
                UpdateScoreText(0);
                return;
            }

            float targetFill = Mathf.Clamp01(currentValue / maxValue);
            scoreFillSlider.DOValue(targetFill, fillDuration).SetEase(fillEase);

            // 텍스트 애니메이션
            if (_scoreTextTween != null && _scoreTextTween.IsActive()) _scoreTextTween.Kill();
            
            _scoreTextTween = DOTween.To(() => _displayScore, x => 
            {
                _displayScore = x;
                UpdateScoreText(_displayScore);
            }, (int)currentValue, fillDuration).SetEase(fillEase);
        }

        // Temp 관련 UI만 업데이트 (슬라이더)
        private void UpdateTempUI()
        {
            if (maxValue <= 0)
            {
                scoreTempFillSlider.DOValue(0, fillDuration).SetEase(fillEase);
                return;
            }

            float targetTempFill = Mathf.Clamp01(tempValue / maxValue);
            scoreTempFillSlider.DOValue(targetTempFill, fillDuration).SetEase(fillEase);
        }

        private void UpdateScoreText(int value)
        {
            if (scoreText != null) scoreText.text = value.ToString("N0");
            if (scoreText2 != null) scoreText2.text = value.ToString("N0");
        }
        
        // 에디터 확인용 및 즉시 갱신
        public void UpdateUIImmediate()
        {
            scoreFillSlider.DOKill();
            scoreTempFillSlider.DOKill();
            if (_scoreTextTween != null) _scoreTextTween.Kill();
            
            if (maxValue <= 0)
            {
                scoreFillSlider.value = 0;
                scoreTempFillSlider.value = 0;
                UpdateScoreText(0);
                return;
            }

            scoreFillSlider.value = Mathf.Clamp01(currentValue / maxValue);
            scoreTempFillSlider.value = Mathf.Clamp01(tempValue / maxValue);
            
            _displayScore = (int)currentValue;
            UpdateScoreText(_displayScore);
        }

        private void OnValidate()
        {
            if (scoreFillSlider == null) return;
            UpdateUIImmediate();
        }
    }
}