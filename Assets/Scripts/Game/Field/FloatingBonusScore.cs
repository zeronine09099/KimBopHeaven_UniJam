using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Field
{
    public class FloatingBonusScore : MonoBehaviour
    {
        public Tile tile;
        [SerializeField] TextMeshProUGUI scoreText;
        private int value;

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
        
        public TextMeshProUGUI ScoreText => scoreText;
        
        public static List<FloatingBonusScore> activeFloatingBonusScores = new();
        
        public bool IsActive => gameObject.activeSelf;
        
        private void OnEnable()
        {
            activeFloatingBonusScores.Add(this);
        }
        private void OnDisable()
        {
            activeFloatingBonusScores.Remove(this);
        }
        
        public static void SetActiveAll(bool isActive)
        {
            foreach (var floatingBonusScore in activeFloatingBonusScores)
            {
                floatingBonusScore.gameObject.SetActive(isActive);
            }
        }
    }
}