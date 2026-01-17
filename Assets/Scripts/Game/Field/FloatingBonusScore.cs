using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Field
{
    public class FloatingBonusScore : MonoBehaviour
    {
        public Tile tile;
        [SerializeField] TMP_Text scoreText;
        private int value;
        [SerializeField] public readonly float[] FontSizes = {30f, 40f, 50f, 60f, 70f};
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                scoreText.text = $"+{value}";
            }
        }

        public float FontSize
        {
            get => scoreText.fontSize;
            set => scoreText.fontSize = value;
        }
        
        public static HashSet<FloatingBonusScore> ActiveScores = new ();

        public void Show()
        {
            isFading = false;
            gameObject.SetActive(true);
            this.DOKill();
            scoreText.DOKill();
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            this.DOKill();
            scoreText.DOKill();
        }
        
        private bool isFading = false;
        
        public bool IsFading => isFading;
        public void FadeOutAndDisable(float duration)
        {
            if (isFading) return;
            isFading = true;
            this.DOKill();
            scoreText.DOKill();
            var seq = DOTween.Sequence();
            seq.Append(scoreText.DOFade(0f, duration));
            seq.AppendCallback(Hide);
            seq.OnComplete(() => isFading = false);
        }
        
        private void OnEnable()
        {
            ActiveScores.Add(this);
        }
        
        private void OnDisable()
        {
            ActiveScores.Remove(this);
        }
    }
}