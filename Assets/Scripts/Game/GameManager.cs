using System;
using System.Collections;
using Common.Singleton;
using Game;
using Game.Field;
using Player;

namespace Core
{
    public class GameManager : Singleton<GameManager>
    {
        
        
        public Field Field { get; set; }
        
        protected override void AfterAwake()
        {
            
        }

        public IEnumerator Init(Action onCompleted = null)
        {
            yield return null;
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