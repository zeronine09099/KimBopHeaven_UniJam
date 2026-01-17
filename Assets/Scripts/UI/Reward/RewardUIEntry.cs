using Game;
using Machamy.Attributes;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Reward
{
    public class RewardUIEntry : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        [SerializeField] private DescriptionModal modal = null;
        [SerializeField] private float holdThreshold;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI baseScore;
        [SerializeField] private TextMeshProUGUI rarity;
        [Header("State")] 
        [SerializeField,VisibleOnly] private IngredientSO ingredientSo;
        [SerializeField,VisibleOnly] private float holdTime = 0f;
        [SerializeField,VisibleOnly] private bool isHolding = false;

        private RewardUI rewardUI;
        public IngredientSO IngredientSo => ingredientSo;

        public void Initialize(RewardUI rewardUI, IngredientSO ingredientSo)
        {
            this.rewardUI = rewardUI;
            this.ingredientSo = ingredientSo;
            iconImage.sprite = ingredientSo.icon;
            nameText.text = ingredientSo.displayName;
            descriptionText.text = ingredientSo.description;
            baseScore.text = ingredientSo.baseScore.ToString();
            rarity.text = $"<{ingredientSo.rarity.ToString()}>";
        }
        
        private void Update()
        {
            if (isHolding)
            {
                holdTime += Time.deltaTime;
                //modal.Show(ingredientSo);
            }
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