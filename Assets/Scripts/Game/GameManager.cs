using System;
using System.Collections;
using Common.Singleton;
using Cysharp.Threading.Tasks;
using Game;
using Game.Field;
using Player;
using UnityEngine;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;
        
        public Field Field { get; set; }
        
        protected override void AfterAwake()
        {
            
        }

        public async UniTask Init(Action onCompleted = null)
        {
            await UniTask.DelayFrame(1);
            IsInitialized = true;
            onCompleted?.Invoke();
        }

        /// <summary>
        /// 플레이어 상태 정보
        /// </summary>
        public PlayerState PlayerStatus { get; set; }

        public void StartGame()
        {
            StartCoroutine(GameRoutine());
        }


        private IEnumerator GameRoutine()
        {
            // 타이틀 정리
            UIManager.Instance.TitleUI.gameObject.SetActive(false);
            
            // 게임 시작
            yield return StageManager.Instance.StartStage();
        }
    }
}