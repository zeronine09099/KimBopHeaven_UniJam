using Game;
using Machamy.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Reward
{
    public class RewardUIEntry : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        [SerializeField] private DescriptionModal modal = null;
        [SerializeField] private float holdThreshold;

        [Space(10)]
        [Header("Selection")]
        [SerializeField] private Image baseImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Color selectedColor;


        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI baseScore;
        [SerializeField] private TextMeshProUGUI rarity;

        [Space(10)]
        [Header("State")] 
        [SerializeField,VisibleOnly] private IngredientSO ingredientSo;
        [SerializeField,VisibleOnly] private float holdTime = 0f;
        [SerializeField,VisibleOnly] private bool isHolding = false;

        [Space(10)]
        [Header("TMP")]
        [SerializeField] TMP_FontAsset font_none;
        [SerializeField] TMP_FontAsset font_normal;
        [SerializeField] TMP_FontAsset font_rare;
        [SerializeField] TMP_FontAsset font_epic;

        private RewardUI rewardUI;
        public IngredientSO IngredientSo => ingredientSo;

        public void Initialize(RewardUI rewardUI, IngredientSO ingredientSo)
        {
            this.rewardUI = rewardUI;
            this.ingredientSo = ingredientSo;
            iconImage.sprite = ingredientSo.icon;
            nameText.text = ingredientSo.displayName;
            descriptionText.text = ingredientSo.description;
            baseScore.text = ingredientSo.ReinforcedBaseScore.ToString();
            rarity.text = $"<{ingredientSo.rarity.ToString()}>";
            switch(ingredientSo.rarity)
            {
                case Common.Rarity.None:
                    rarity.font = font_none;
                    break;
                case Common.Rarity.Normal:
                    rarity.font = font_normal;
                    break;
                case Common.Rarity.Rare:
                    rarity.font = font_rare;
                    break;
                case Common.Rarity.Epic:
                    rarity.font = font_epic;
                    break;
            }
        }
        
        private void Update()
        {
            if (isHolding)
            {
                holdTime += Time.deltaTime;
                //modal.Show(ingredientSo);
            }
        }

        public void SelectedVisualEffect()
        {
            baseImage.color = selectedColor;
        }

        public void DeselectVisualEffect()
        {
            baseImage.color = Color.white;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHolding = true;
            holdTime = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHolding = false;
            Debug.Log($"Pointer released after holding for {holdTime} seconds.");
            if (holdTime >= holdThreshold)
            {
                modal.Show(ingredientSo);
            }
            else if (holdTime <= 0.2f)
            {
                rewardUI.OnEntryClicked(this);
            }
            else
            {
                //modal.Hide();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHolding = false;
            Debug.Log($"Pointer exited after holding for {holdTime} seconds.");
            //modal.Hide();
        }
    }
}