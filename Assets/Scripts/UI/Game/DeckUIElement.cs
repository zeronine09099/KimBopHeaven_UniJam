using Game;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Game
{
    public class DeckUIElement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Button addButton;
        [SerializeField] private Button deleteButton;
        [Header("Setting")]
        [Header("Variables")]
        public IngredientSO ingredient;

        public void Initialize(IngredientSO ingredient)
        {
            this.ingredient = ingredient;
        }
        
        public void SetDeckForm()
        {
            iconImage.sprite = ingredient.icon;
            addButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }

        public void SetDeleteForm()
        {
            iconImage.sprite = ingredient.icon;
            addButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(true);
        }
        
        public void Refresh()
        {
            int count = PlayerState.Current.GameDeck.GetIngredientCount(ingredient);
            countText.text = count.ToString();
        }

        public void SwitchDescActivation()
        {
            FindAnyObjectByType<DeckUI>().SwitchDesc(this);
        }
    }
}