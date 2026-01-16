using System;
using System.Collections;
using System.Threading;
using Common.Singleton;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using Machamy.DeveloperConsole.Attributes;
using Machamy.Utils;
using Player;
using UnityEngine;

namespace Game
{
    public class StageManager : Singleton<StageManager>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;

        public StageInfo CurrentStageInfo => PlayerState.Current.CurrentStageInfo;
        public int CurrentStageId => PlayerState.Current.CurrentStageInfo.Stage;
        
        protected override void AfterAwake()
        {
            
        }
        
        public async UniTask Init(Action onCompleted = null)
        {
            LogEx.Log("Initializing StageManager...");
            await UniTask.Yield();
            onCompleted?.Invoke();
            LogEx.Log("StageManager initialized.");
            IsInitialized = true;
        }

        /// <summary>
        /// 강제 종료용 토큰 소스
        /// </summary>
        CancellationTokenSource _forceStopCts = new CancellationTokenSource();
        
        /// <summary>
        /// 스테이지 진행용 토큰 소스
        /// </summary>
        CancellationTokenSource _stageCts = new CancellationTokenSource();
        
        private void ForceStopStage()
        {
            _forceStopCts?.Cancel();
            _forceStopCts?.Dispose();
            _forceStopCts = new CancellationTokenSource();
            
            _stageCts?.Cancel();
            _stageCts?.Dispose();
            _stageCts = new CancellationTokenSource();
        }
        
        public async UniTask StartStage(StageInfo stage, CancellationToken cancellationToken = default)
        {
            _stageCts?.Dispose();
            _stageCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _forceStopCts.Token);
            var linkedToken = _stageCts.Token;
            PlayerState.Current.CurrentStageInfo = stage;
            /*
             * 초기화 단계
             */
            
            // 스테이지 데이터 초기화
            Root.Field.InitField(6,6);
            
            
            /*
             * 연출 단계
             */

            await UniTask.Yield(linkedToken);

            TurnLoop(linkedToken).Forget();
        }

        public async UniTask TurnLoop(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                // 각 턴마다 처리할 로직 작성
                await UniTask.Yield();
                
                
                // 클리어 체크
                // if(PlayerState.Current.CurrentStageTarget.IsCleared())
                // {
                //     await StageSuccess();
                //     break;
                // }
                // 실패 체크
                if(PlayerState.Current.CurrentRemainingSwipes <= 0)
                {
                    await StageFail();
                    break;
                }
                
            }
        }
        
        
        
        public async UniTask StageSuccess()
        {
            // 스테이지 성공 처리, 리워드로
            await UniTask.Yield();
            
            await UIManager.Instance.RewardUI.ShowAsync(CurrentStageInfo);
            
            // 이벤트
            
            // 다음 스테이지로
            StartStage(StageLibrary.Instance.GetNextStageInfo(CurrentStageId), _forceStopCts.Token).Forget();
        }
        
        public async UniTask StageFail()
        {
            // 스테이지 실패 처리, 타이틀로
            await UniTask.Yield();
            
            
            GameManager.Instance.CancelGameAndReturnToTitle();
        }
        
        

        
        [ConsoleCommand("stage.clear")]
        public static void StageClearCommand()
        {
            var sm = Instance;
            sm.ForceStopStage();
            sm.StageSuccess().Forget();
            
        }

        [ConsoleCommand("stage.fail")]
        public static void StageFailCommand()
        {
            var sm = Instance;
            sm.ForceStopStage();
            sm.StageFail().Forget();
            
        }
    }
}