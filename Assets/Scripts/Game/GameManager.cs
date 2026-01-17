using System;
using System.Collections;
using System.Threading;
using Common.Singleton;
using Cysharp.Threading.Tasks;
using Game;
using Game.Field;
using Machamy.Utils;
using Player;
using Sound;
using UnityEngine;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;
        
        public Field Field { get; set; }
        
        
        private CancellationTokenSource _gameCancellationTokenSource = new CancellationTokenSource();
        
        protected override void AfterAwake()
        {
            
        }

        public async UniTask Init(Action onCompleted = null)
        {
            LogEx.Log("Initializing GameManager...");
            await UniTask.DelayFrame(1);
            IsInitialized = true;
            LogEx.Log("GameManager Initialized.");
            onCompleted?.Invoke();

            SoundManager.Instance.PlayBackgroundMusic(SoundReference.BackgroundMusic);
        }

        /// <summary>
        /// 플레이어 상태 정보
        /// </summary>
        [field:SerializeField]public PlayerState PlayerStatus { get; private set; } = new PlayerState();

        public void StartGame()
        {
            _gameCancellationTokenSource = new CancellationTokenSource();
            UIManager.Instance.GoToInGameUI();
            GameRoutine(_gameCancellationTokenSource.Token).Forget();
        }

        public void CancelGame()
        {
            _gameCancellationTokenSource.Cancel();
            _gameCancellationTokenSource.Dispose();
            _gameCancellationTokenSource = new CancellationTokenSource();
        }
        
        public void CancelGameAndReturnToTitle()
        {
            CancelGame();
            UIManager.Instance.GoToTitleUI();
        }
        
        private async UniTask GameRoutine(CancellationToken cancellationToken)
        {
            // 타이틀 정리
            UIManager.Instance.TitleUI.gameObject.SetActive(false);
            
            // 플레이어 상태 초기화
            PlayerStatus.SetUpNewGame();
            
            // 게임 루프
            await StageManager.Instance.StartStage(PlayerStatus.CurrentStageInfo, cancellationToken);
            
        }   
    }
}