using TMPro;
using UnityEngine;

namespace Game.Field
{
    public class FloatingBonusScore : MonoBehaviour
    {
        public Tile tile;
        [SerializeField] TMP_Text scoreText;
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
    }
}