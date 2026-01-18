using System;
using System.Security.Cryptography;
using System.Threading;
using BandoWare.GameplayTags;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using Player;
using UnityEngine;

namespace UI.Encounter
{
    public class EncounterUI : MonoBehaviour
    {
        [SerializeField] DeliveryEncounterPanel deliveryEncounterPanel;
        [SerializeField] DeliveryEncounterPanel blackCookEncounterPanel;

        private bool encounterEnded = false;
        public bool SelectTrashEnded { get; set; }

        private void Awake()
        {
            UIManager.Instance.EncounterUI = this;
            gameObject.SetActive(false);
        }

        private void ShowDelivery()
        {
            Debug.Log("showdelivery 들어옴"); 
            int temp1, temp2;

            while(true)
            {
                temp1 = UnityEngine.Random.Range(0, 3);
                temp2 = UnityEngine.Random.Range(0, 3);
                if(temp1 != temp2)
                {
                    break;
                }
            }
            deliveryEncounterPanel.Initialize(UIManager.Instance.EncounterUI, temp1, temp2);
            deliveryEncounterPanel.gameObject.SetActive(true);
        }

        private void ShowBlackCook()
        {
            Debug.Log("showblackcook 들어옴");
            blackCookEncounterPanel.gameObject.SetActive(true);
        }

        public async UniTask ShowAsync(StageInfo stageInfo, CancellationToken cancellationToken = default)
        {
            gameObject.SetActive(true);
            if (stageInfo.Stage == 3 || stageInfo.Stage == 9 || stageInfo.Stage == 15)
            {
                // 납품 강화 이벤트
                ShowDelivery(); 
            }
            else if(stageInfo.Stage == 6 || stageInfo.Stage == 12 || stageInfo.Stage == 18)
            {
                // 흑종원 이벤트
                Debug.Log("흑종원들어옴");
                ShowBlackCook();
            }

            while(encounterEnded == false)
            {
                await UniTask.Yield(cancellationToken); 
            }
            Debug.Log("showasync 끝남");

        }

        public async UniTask destroyIngredientAsync(CancellationToken cancellationToken = default)
        {
            //remove 버튼 활성화
            UIManager.Instance.DeckUI.ActivateRemoveMode(); 

            while (SelectTrashEnded == false) 
            {
                await UniTask.Yield(cancellationToken);
            }
            Debug.Log("destroyINgredient 끝남");
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
            GameplayTag upgradeTargetTag = deliveryEncounterPanel.UpgradeTargetTag;
            int upgradeValue = deliveryEncounterPanel.UpgradeValue;
            PlayerState.Current.Reinforcements.AddReinforcement(upgradeTargetTag, upgradeValue);
            Hide();
            return;
        }

        public void OnBlackCookEncounterClick()
        {
            // 가방열고 가방버리는 로직
            // 다음 스테이지로 넘어가기
            UIManager.Instance.DeckUI.ShowDeckForm();
            UIManager.Instance.EncounterUI.gameObject.SetActive(true);
            destroyIngredientAsync().ContinueWith(Callback).Forget();   // callback 이 유니테스크가 끝난 뒤에 실행
            void Callback()
            {
                encounterEnded = true;
                blackCookEncounterPanel.gameObject.SetActive(false);
                Hide();
            }
            return;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}