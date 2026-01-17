using Core;
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
        [SerializeField] private Button MinusButton;
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
            MinusButton.gameObject.SetActive(false);
        }

        public void SetDeleteForm()
        {
            iconImage.sprite = ingredient.icon;
            addButton.gameObject.SetActive(false);
            MinusButton.gameObject.SetActive(true);
        }
        
        // 흑종원 encounter에서만 remove 버튼이 활성화된다.
        public void ActivateMinusButton()
        {
            MinusButton.gameObject.SetActive(true);
        }

        // 활성화된 삭제 버튼을 누르면 해당 재료의 개수가 1개 줄어든다.
        public void OnClickMinusButton()
        {
            Debug.Log("마이너스 버튼 클릭 들어옴");
            if( PlayerState.Current.GameDeck.GetIngredientCount(ingredient) <=0 )
            {
                Debug.Log("재료 개수가 0보다작음");
                return;
            }

            PlayerState.Current.GameDeck.RemoveIngredient(ingredient);
            Refresh();
            UIManager.Instance.DeckUI.Hide();
            UIManager.Instance.EncounterUI.SelectTrashEnded = true;
            return;
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