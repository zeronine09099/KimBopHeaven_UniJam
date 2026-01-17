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
        [SerializeField] EncounterImagePanel DeliveryEncounterPanel;
        [SerializeField] EncounterImagePanel secondEncounterPanel;

        private bool encounterEnded = false;
        private void Awake()
        {
            UIManager.Instance.EncounterUI = this;
            gameObject.SetActive(false);
        }

        public async UniTask ShowAsync(StageInfo stageInfo, CancellationToken cancellationToken = default)
        {
            if (stageInfo.Stage == 5 || stageInfo.Stage == 15)
            {
               // 납품 강화 이벤트
            }
            else if(stageInfo.Stage == 10 || stageInfo.Stage == 20)
            {
                // 흑종원 이벤트
            }

            while(encounterEnded == false)
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

        public void OnEncounterClick()
        {

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}