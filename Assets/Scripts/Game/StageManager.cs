using System;
using System.Collections;
using Common.Singleton;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class StageManager : Singleton<StageManager>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;

        
        protected override void AfterAwake()
        {
            
        }
        
        public async UniTask Init(Action onCompleted = null)
        {
            await UniTask.Yield();
            onCompleted?.Invoke();
            IsInitialized = true;
        }

        public IEnumerator StartStage()
        {
            /*
             * 초기화 단계
             */
            
            // 스테이지 데이터 초기화
            Root.Field.InitField(6,6);
            
            
            /*
             * 연출 단계
             */

            yield return null;
        }
        
        
        
        public IEnumerator StageSuccess()
        {
            // 스테이지 성공 처리, 리워드로
            yield return null;
        }
        
        public IEnumerator StageFail()
        {
            // 스테이지 실패 처리, 타이틀로
            yield return null;
        }
    }
}