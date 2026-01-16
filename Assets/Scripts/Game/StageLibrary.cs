using System;
using Common.Collections;
using Common.Singleton;
using Cysharp.Threading.Tasks;
using Database;
using Database.Generated;
using Machamy.Attributes;
using Machamy.Utils;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 스테이지 라이브러리
    /// </summary>
    public class StageLibrary : Singleton<StageLibrary>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;

        
        [VisibleOnly,SerializeField] SerializableDictionary<int, StageInfo> stages = new SerializableDictionary<int, StageInfo>();
        
        private StageInfo _lastStage;
        
        protected override void AfterAwake()
        {
        }
        
        public async UniTask Init(Action onCompleted = null)
        {
            await UniTask.WaitUntil(() => DatabaseManager.Instance.IsInitialized);
            var stageTable = DatabaseManager.Instance.Database.StageInfoList;
            stages.Clear();
            foreach (var stage in stageTable)
            {
                stages.Add(stage.Stage, stage);
                if (_lastStage == null || stage.Stage > _lastStage.Stage)
                {
                    _lastStage = stage;
                }   
            }
            IsInitialized = true;
            LogEx.Log($"StageLibrary initialized with {stages.Count} stages.");
            onCompleted?.Invoke();
        }
        
        public StageInfo GetStageInfo(int stageId)
        {
            if (stages.TryGetValue(stageId, out var stageInfo))
            {
                return stageInfo;
            }

            if (stageId == 0)
            {
                return GetStageInfo(1);
            }

            return GenerateStageInfo(stageId);
        }


        public static StageInfo GenerateStageInfo(int stageId)
        {
            var lastData = Instance._lastStage;
            var nextData = new StageInfo();
            int levelDiff = stageId - lastData.Stage;
            int positiveLevelDiff = Math.Max(0, levelDiff);
            
            nextData.Stage = stageId;
            
            // 마지막 단계 점수를 기준으로 
            nextData.goalScore = Mathf.RoundToInt(lastData.goalScore + levelDiff * 50000 * (MathF.Pow(1.1f, positiveLevelDiff)));
            
            // 마지막 단계를 기준으로 5레벨마다 1회 추가
            nextData.moveCount = lastData.moveCount + (levelDiff / 5);
            
            nextData.normalPercentage = lastData.normalPercentage;
            nextData.rarePercentage = lastData.rarePercentage;
            nextData.epicPercentage = lastData.epicPercentage;
            
            return nextData;
        }

        public StageInfo GetNextStageInfo(int currentStageId)
        {
            return GetStageInfo(currentStageId + 1);
        }
    }
}