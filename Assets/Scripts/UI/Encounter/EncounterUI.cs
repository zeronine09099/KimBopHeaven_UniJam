using System;
using System.Security.Cryptography;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using UnityEngine;

namespace UI.Encounter
{
    public class EncounterUI : MonoBehaviour
    {
        [SerializeField] DeliveryEncounterPanel deliveryEncounterPanel;
        [SerializeField] DeliveryEncounterPanel blackCookEncounterPanel;

        private bool encounterEnded = false;
        private bool selectTrashEnded = false;
        private void Awake()
        {
            UIManager.Instance.EncounterUI = this;
            gameObject.SetActive(false);
        }

        private void ShowDelivery()
        {
            int temp1, temp2;

            while(true)
            {
                temp1 = UnityEngine.Random.Range(0, 3);
                temp2 = UnityEngine.Random.Range(0, 3);
                if(temp1 == temp2)
                {
                    break;
                }
            }
            deliveryEncounterPanel.Initialize(UIManager.Instance.EncounterUI, temp1, temp2);
            deliveryEncounterPanel.gameObject.SetActive(true);
        }

        private void ShowBlackCook()
        {
            blackCookEncounterPanel.gameObject.SetActive(true);
        }

        public async UniTask ShowAsync(StageInfo stageInfo, CancellationToken cancellationToken = default)
        {
            if (stageInfo.Stage == 5 || stageInfo.Stage == 15)
            {
                // 납품 강화 이벤트
                ShowDelivery(); 
            }
            else if(stageInfo.Stage == 10 || stageInfo.Stage == 20)
            {
                // 흑종원 이벤트
                ShowBlackCook();
            }

            while(encounterEnded == false)
            {
                await UniTask.Yield(cancellationToken); 
            }

        }

        public async UniTask destroyIngredientAsync(CancellationToken cancellationToken = default)
        {


            while (selectTrashEnded == false) 
            {
                await UniTask.Yield(cancellationToken);
            }
        }

        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }

        public void OnDeliveryEncounterClick()
        {
            // 강화로직 적용
            // 다음 스테이지로 넘어가기
            encounterEnded = true;
            Hide();
            return;
        }

        public void OnBlackCookEncounterClick()
        {
            // 가방열고 가방버리는 로직
            // 다음 스테이지로 넘어가기
            UIManager.Instance.DeckUI.ShowDeckForm();
            destroyIngredientAsync().Forget();
            encounterEnded = true;
            Hide();
            return;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}