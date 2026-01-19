using System;
using System.Collections;
using System.Threading;
using Common.Singleton;
using Core;
using Cysharp.Threading.Tasks;
using Database.Generated;
using Game.Field;
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
            GameManager.Instance.Field.DestroyIngredients();
            PlayerState.Current.CurrentStageInfo = stage;
            /*
             * 초기화 단계
             */
            
            var um = UIManager.Instance;
            var gameUI = um.InGameUI;
            
            // 플레이어 스테이트 초기화
            PlayerState.Current.ResetByPrefix("Current");
            // 플레이어 스테이지 정보 세팅(이벤트 호출용)
            PlayerState.Current.CurrentRemainingSwipes = stage.moveCount;
            PlayerState.Current.CurrentStageScore = 0;
            PlayerState.Current.CurrentTempScore = 0;
            PlayerState.Current.CurrentRerollRemain = 3;

            // 스테이지 데이터 초기화
            Root.Field.InitField();

            // UI 점수 초기화
            gameUI.InitializeForStage(stage);
            PuzzleManager.Instance.Initialize(GameManager.Instance.Field);
            PuzzleManager.Instance.CurrentMatches.Clear();
            

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
                int scoreSnapshot = PlayerState.Current.CurrentStageScore;
                // 각 턴마다 처리할 로직 작성
                PuzzleManager.Instance.ThisTurnCompletedKimbapCount = 0;
                LogEx.Log($"Starting new turn. Remaining Swipes: {PlayerState.Current.CurrentRemainingSwipes}");
                PlayerInputData inputData = await PuzzleManager.Instance.GetPlayerInput(cancellationToken);
                LogEx.Log($"Player input received: {inputData.firstTile} <-> {inputData.secondTile}");
                if (!PuzzleManager.Instance.IsValidSwap(inputData.firstTile, inputData.secondTile))
                {
                    // 유효하지 않은 스왑인 경우, 다시 입력 받기
                    LogEx.Log("Invalid swap. Requesting input again.");
                    continue;
                }
                LogEx.Log("Processing player input...");
                var matches = await PuzzleManager.Instance.ProcessInput(inputData, cancellationToken);
                LogEx.Log($"Matches found: {matches.Count}");
                while (matches.Count > 0){
                    
                    await PuzzleManager.Instance.ProcessMatches(matches, cancellationToken);
                    LogEx.Log("Turn processing complete.");
                    GameManager.ResetTimeScale();
                    
                    await PuzzleManager.Instance.WrapUpTurn(cancellationToken);
                    LogEx.Log("Turn wrapped up.");
                    
                    matches = PuzzleManager.Instance.FindWrapperMatches();
                    LogEx.Log($"New matches found: {matches.Count}");
                }
                // 턴 종료 후 점수 갱신
                PlayerState.Current.BestSwipeKimbapCount = Mathf.Max(PlayerState.Current.BestSwipeKimbapCount, PuzzleManager.Instance.ThisTurnCompletedKimbapCount);
                
                PlayerState.Current.CurrentStageSwipeCount += 1;
                PlayerState.Current.TotalSwipeCount += 1;
                
                PlayerState.Current.BestSingleSwipeScore = Mathf.Max(PlayerState.Current.BestSingleSwipeScore, scoreSnapshot);
                
                // 클리어 체크
                if(PlayerState.Current.CurrentStageScore >= PlayerState.Current.CurrentStageInfo.goalScore)
                {
                    await StageSuccess(cancellationToken);
                    break;
                }
                PlayerState.Current.CurrentRemainingSwipes -= 1;
                // 실패 체크
                if(PlayerState.Current.CurrentRemainingSwipes <= 0)
                {
                    await StageFail(cancellationToken);
                    break;
                }
                
            }
        }
        
        
        
        public async UniTask StageSuccess(CancellationToken cancellationToken = default)
        {
            // 스테이지 성공 처리, 리워드로
            LogEx.Log("Stage Cleared!");
            await UniTask.Yield();
            
            // 통계 처리
            PlayerState ps = PlayerState.Current;

            CheckBestScores();
            
            PlayerState pl = PlayerState.Current;
            pl.TotalScore += pl.CurrentStageScore;
            
            await UIManager.Instance.BillingUI.ShowSuccessAsync(cancellationToken);
            
            await UIManager.Instance.BillingUI.WaitForSkip(cancellationToken);
            
            await UIManager.Instance.BillingUI.HideAsync(cancellationToken);
            
            await UIManager.Instance.RewardUI.ShowAsync(CurrentStageInfo,cancellationToken);

            if(CurrentStageInfo.Stage % 3 == 0)
            {
                Debug.Log("stagemanager 인카운터 분기 들어옴");
                await UIManager.Instance.EncounterUI.ShowAsync(CurrentStageInfo);
            }
            // 이벤트
            
            // 다음 스테이지로
            GameManager.Instance.Field.DestroyIngredients();
            StartStage(StageLibrary.Instance.GetNextStageInfo(CurrentStageId), cancellationToken).Forget();
        }
        
        public async UniTask StageFail(CancellationToken cancellationToken = default)
        {
            // 스테이지 실패 처리, 타이틀로
            LogEx.Log("Stage Failed!");
            await UIManager.Instance.BillingUI.ShowFailAsync(cancellationToken);

            // TODO : 실패한 스테이지는 경신하나??
            CheckBestScores();
            PlayerState pl = PlayerState.Current;
            
            pl.TotalScore += pl.CurrentStageScore;

            //GameManager.Instance.CancelGameAndReturnToTitle();
            await UIManager.Instance.HideAll();
            await UIManager.Instance.GameoverUI.ShowAsync(cancellationToken);
            await UIManager.Instance.GameoverUI.WaitForHide(cancellationToken);
            
        }


        private void CheckBestScores()
        {
            PlayerState pl = PlayerState.Current;
            
            pl.BestStageKimbapCount = Mathf.Max(pl.BestStageKimbapCount, pl.CurrentStageKimbapCount);
            pl.BestStageScore = Mathf.Max(pl.BestStageScore, pl.CurrentStageScore);
            pl.BestStageSwipeCount = Mathf.Max(pl.BestStageSwipeCount, pl.CurrentStageInfo.moveCount - pl.CurrentRemainingSwipes);
            //pl.BestSingleSwipeScore 는 턴이 끝날 때마다 갱신
            pl.BestStageClearedCount = Mathf.Max(pl.BestStageClearedCount, pl.CurrentStageClearedCount);
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