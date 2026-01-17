using Common;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using Game;
using Player;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace UI.Reward
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] RewardUIEntry[] entries;
        [SerializeField] TextMeshProUGUI rerollBtnText;
        [SerializeField] TextMeshProUGUI remainSelectBtnText;
        private RewardUIEntry selectedRewardUIEntry;
        private int remainRerollCnt = 3;
        private int remainSelectCnt = 3;
        
        private void Awake()
        {
            UIManager.Instance.RewardUI = this;
            gameObject.SetActive(false);
        }

        public void Show(StageInfo stageInfo)
        {
            remainSelectBtnText.text = $"{remainSelectCnt}";
            rerollBtnText.text = $"리롤 ({remainRerollCnt})";

            foreach(var e in entries)
            {
                e.DeselectVisualEffect();
            }
            
            Rarity GetRarity()
            {
                Rarity[] rarities = new[] { Rarity.None, Rarity.Normal, Rarity.Rare, Rarity.Epic };
                int idx = Utilities.WeightedRandomIndex(0f,
                    stageInfo.normalPercentage,
                    stageInfo.rarePercentage,
                    stageInfo.epicPercentage);
                return rarities[idx];
            }

            for (int i = 0; i < 3; i++)
            {
                var rarity = GetRarity();
                var ingredient = IngredientLibrary.Instance.GetRandomIngredientSOByRarity(rarity);
                entries[i].Initialize(this, ingredient);
            }
        }

        public async UniTask ShowAsync(StageInfo stageInfo, CancellationToken cancellationToken = default)
        {
            remainRerollCnt = 3; 
            remainSelectCnt = 3;
            Show(stageInfo);
            gameObject.SetActive(true);
            rewardSelectEnded = false;
            while (!rewardSelectEnded)
            {
                await UniTask.Yield(cancellationToken);
            }
        }
        
        private bool rewardSelectEnded = false;
        public void OnEntryClicked(RewardUIEntry entry)
        {
            foreach(var e in entries)
            {
                e.DeselectVisualEffect();
            }

            if (entry == selectedRewardUIEntry)
            {
                selectedRewardUIEntry = null;
                rewardSelectEnded = true;
                return;
            }

            //Debug.Log($"RewardUI: OnEntryClicked - {entry.IngredientSo.name}");
            selectedRewardUIEntry = entry;
            entry.SelectedVisualEffect();
        }

        public void OnRemainSelectionCntButton()
        {

        }

        public void OnRerollButton()
        {
            if(remainRerollCnt >0)
            {
                remainRerollCnt--;
                Show(PlayerState.Current.CurrentStageInfo);
            }
            else
            {
                return;
            }
        }   
        
        public void SelectButton()
        {
            if(remainSelectCnt > 0)
            {
                PlayerState.Current.GameDeck.AddIngredient(selectedRewardUIEntry.IngredientSo);
                remainSelectCnt--;
                if(remainSelectCnt == 0)
                {
                    rewardSelectEnded = true;
                    Hide();
                    return;
                }
                Show(PlayerState.Current.CurrentStageInfo);
                return;
            }
            else
            {
                // 게임 시작 로직
                rewardSelectEnded = true;
                Hide();
            }
        }

        public void ViewDeckButton()
        {
            UIManager.Instance.DeckUI.ShowDeckForm();
        }

        public void SkipButton()
        {

            if (remainSelectCnt > 0)
            {
                remainSelectCnt--;
                if(remainSelectCnt == 0)
                {
                    // 다음 스테이지 시작 로직
                    rewardSelectEnded = true;
                    Hide();
                }
                Show(PlayerState.Current.CurrentStageInfo);
                return;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    
}