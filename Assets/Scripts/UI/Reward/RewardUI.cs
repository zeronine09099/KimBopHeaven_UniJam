using System;
using System.Threading;
using Common;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using Game;
using Player;
using UnityEngine;

namespace UI.Reward
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] RewardUIEntry[] entries;
        
        private void Awake()
        {
            UIManager.Instance.RewardUI = this;
            gameObject.SetActive(false);
        }


        public void Show(StageInfo stageInfo)
        {
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
            Show(stageInfo);
            gameObject.SetActive(true);
            buttonClicked = false;
            while (!buttonClicked)
            {
                await UniTask.Yield(cancellationToken);
            }
        }
        
        private bool buttonClicked = false;
        public void OnEntryClicked(RewardUIEntry entry)
        {
            Debug.Log($"RewardUI: OnEntryClicked - {entry.IngredientSo.name}");
            PlayerState.Current.Inventory.AddIngredient(entry.IngredientSo);
            buttonClicked = true;
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    
}