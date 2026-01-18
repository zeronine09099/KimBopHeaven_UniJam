using System;
using System.Collections.Generic;
using Common;
using Core;
using Database.Generated;
using Game;
using Machamy.DeveloperConsole;
using Machamy.DeveloperConsole.Attributes;
using Machamy.DeveloperConsole.Commands;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable ArrangeAccessorOwnerBody
namespace Player
{
    /// <summary>
    /// 플레이어의 상태(스코어, 코인 등)를 관리하는 클래스입니다.
    /// - 내부 데이터는 <see cref="VariableContainer"/>에 저장됩니다.
    /// - 외부 접근/수정은 반드시 이 클래스의 Property를 통해 이루어져야 합니다. (이벤트 관리/변경 감지를 위해)
    ///
    /// 접두사 규칙:
    /// - Total : 해당 게임을 진행하면서 누적된 합계 값
    /// - Best  : 해당 게임에서의 최고값
    /// - BestStage : 진행한 여러 스테이지 중 최고값
    /// - Current : 현재 실시간 값
    /// </summary>
    [Serializable]
    public class PlayerState
    {
        public static PlayerState Current => GameManager.Instance.PlayerStatus;
        
        [SerializeField] private VariableContainer variables = new VariableContainer();
        
        [FormerlySerializedAs("inventory")] [SerializeField] private Inventory gameDeck = new Inventory();
        
        [SerializeField] ReinforcementContainer reinforcementContainer = new ReinforcementContainer();
        
        
        /// <summary>
        /// 게임 시작 시 초기화되는 인벤토리(덱)입니다.
        /// </summary>
        public Inventory GameDeck => gameDeck;
        
        public ReinforcementContainer Reinforcements => reinforcementContainer;
        
        
        
        /// <summary>
        /// 내부 VariableContainer 인스턴스입니다. 외부에서 읽을 수 있고 설정은 이 클래스 내부에서만 가능합니다.
        /// Variable에 접근/수정할 때는 이 프로퍼티를 사용하세요.
        /// </summary>
        public VariableContainer Variables
        {
            get => variables;
            private set => variables = value;
        }
        public void Reset()
        {
            Variables = new VariableContainer();
            foreach (VariableKey key in Enum.GetValues(typeof(VariableKey)))
            {
                // Multiplier는 실수형이므로 제외하거나 1.0f로 초기화, 나머지는 0
                if (key == VariableKey.CurrentTempMultiplier)
                    Variables.SetFloat(key.ToString(), 1.0f);
                else
                    Variables.SetInteger(key.ToString(), 0);
            }
            CurrentStageInfo = null;
        }

        public enum VariableKey
        {
            None,
            // 1. 김밥 개수 (Kimbap Count)
            TotalKimbapCount,         // 게임 전체 누적 김밥
            BestStageKimbapCount,     // 한 스테이지에서 만든 최대 김밥 기록
            CurrentStageKimbapCount,  // 현재 스테이지 김밥 개수
            BestSwipeKimbapCount,     // 한 스와이프로 만든 김밥 개수 최고 기록

            // 2. 점수 (Score)
            TotalScore,               // 게임 전체 누적 점수
            BestStageScore,           // 한 스테이지 최고 점수 기록
            CurrentStageScore,        // 현재 스테이지 점수
            CurrentTempScore,         // 스와이프 중 임시 점수

            // 3. 스와이프 수 (Swipe Count)
            TotalSwipeCount,          // 게임 전체 누적 스와이프
            BestStageSwipeCount,      // 한 스테이지에서 기록한 스와이프 (오래 버티기 기록 등)
            CurrentStageSwipeCount,   // 현재 스테이지에서 사용한 스와이프 수
            CurrentRemainingSwipes,   // 현재 남은 스와이프 횟수

            // 4. 지운 재료 수 (Cleared Elements)
            TotalClearedCount,        // 게임 전체 누적 지운 개수
            BestStageClearedCount,    // 한 스테이지 최다 지운 개수 기록
            CurrentStageClearedCount, // 현재 스테이지 지운 개수

            // 5. 기타 및 제안 (Misc & Suggestions)
            CurrentTempMultiplier,    // 현재 임시 멀티플라이어 (float)
            BestSingleSwipeScore,     // 한 번의 스와이프로 얻은 역대 최고 점수
            MaxComboCount,            // 한 번의 스와이프에서 터진 최대 연쇄 횟수
        }

        // --- PlayerStatus Properties ---

        #region 1. Kimbap Count
        /// <summary>
        /// 게임 진행 중 누적된 총 김밥 횟수
        /// </summary>
        public int TotalKimbapCount
        {
            get => Variables.GetVariable(nameof(VariableKey.TotalKimbapCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.TotalKimbapCount), value);
        }

        /// <summary>
        /// 단일 스테이지에서 기록한 최고 김밥 횟수
        /// </summary>
        public int BestStageKimbapCount
        {
            get => Variables.GetVariable(nameof(VariableKey.BestStageKimbapCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestStageKimbapCount), value);
        }

        /// <summary>
        /// 현재 스테이지에서 만든 김밥 횟수
        /// </summary>
        public int CurrentStageKimbapCount
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentStageKimbapCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.CurrentStageKimbapCount), value);
        }

        public int BestSwipeKimbapCount
        {
            get => Variables.GetVariable(nameof(VariableKey.BestSwipeKimbapCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestSwipeKimbapCount), value);
        }
        
        
        
        #endregion

        #region 2. Score
        /// <summary>
        /// 게임 진행 중 누적된 총 점수
        /// </summary>
        public int TotalScore
        {
            get => Variables.GetVariable(nameof(VariableKey.TotalScore)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.TotalScore), value);
        }

        /// <summary>
        /// 단일 스테이지 최고 점수
        /// </summary>
        public int BestStageScore
        {
            get => Variables.GetVariable(nameof(VariableKey.BestStageScore)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestStageScore), value);
        }

        /// <summary>
        /// 현재 스테이지 점수 (UI 갱신 포함)
        /// </summary>
        public int CurrentStageScore
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentStageScore)).IntValue;
            set
            {
                Variables.SetInteger(nameof(VariableKey.CurrentStageScore), value);
                // UI 갱신 로직
                if (UIManager.Instance?.InGameUI?.ScoreUI != null)
                    UIManager.Instance.InGameUI.ScoreUI.CurrentValue = value;
            }
        }

        /// <summary>
        /// 현재 계산 중인 임시 점수 (UI 갱신 및 효과음 포함)
        /// </summary>
        public int CurrentTempScore
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentTempScore)).IntValue;
            set
            {
                bool isIncreased = value > CurrentTempScore;
                Variables.SetInteger(nameof(VariableKey.CurrentTempScore), value);
                
                if (UIManager.Instance?.InGameUI?.ScoreUI != null)
                    UIManager.Instance.InGameUI.ScoreUI.TempValue = value;

                if (isIncreased)
                {
                    SoundManager.Instance.PlayScoreSfx();
                }
            }
        }
        #endregion

        #region 3. Swipe Count
        /// <summary>
        /// 게임 진행 중 누적된 총 스와이프 횟수
        /// </summary>
        public int TotalSwipeCount
        {
            get => Variables.GetVariable(nameof(VariableKey.TotalSwipeCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.TotalSwipeCount), value);
        }

        /// <summary>
        /// 단일 스테이지 최다 스와이프 기록
        /// </summary>
        public int BestStageSwipeCount
        {
            get => Variables.GetVariable(nameof(VariableKey.BestStageSwipeCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestStageSwipeCount), value);
        }

        /// <summary>
        /// 현재 스테이지에서 사용한 스와이프 횟수
        /// </summary>
        public int CurrentStageSwipeCount
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentStageSwipeCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.CurrentStageSwipeCount), value);
        }

        /// <summary>
        /// 현재 남은 스와이프 횟수 (UI 갱신 포함)
        /// </summary>
        public int CurrentRemainingSwipes
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentRemainingSwipes)).IntValue;
            set
            {
                Variables.SetInteger(nameof(VariableKey.CurrentRemainingSwipes), value);
                if (UIManager.Instance?.InGameUI != null)
                    UIManager.Instance.InGameUI.OnRemainingMovesChanged(value);
            }
        }
        #endregion

        #region 4. Cleared Elements
        /// <summary>
        /// 게임 진행 중 누적된 총 삭제 재료 수
        /// </summary>
        public int TotalClearedCount
        {
            get => Variables.GetVariable(nameof(VariableKey.TotalClearedCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.TotalClearedCount), value);
        }

        /// <summary>
        /// 단일 스테이지 최다 삭제 재료 기록
        /// </summary>
        public int BestStageClearedCount
        {
            get => Variables.GetVariable(nameof(VariableKey.BestStageClearedCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestStageClearedCount), value);
        }

        /// <summary>
        /// 현재 스테이지에서 삭제한 재료 수
        /// </summary>
        public int CurrentStageClearedCount
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentStageClearedCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.CurrentStageClearedCount), value);
        }
        #endregion

        #region 5. Misc & Suggestions
        /// <summary>
        /// 현재 임시 멀티플라이어 (Float)
        /// </summary>
        public float CurrentTempMultiplier
        {
            get => Variables.GetVariable(nameof(VariableKey.CurrentTempMultiplier)).FloatValue;
            set => Variables.SetFloat(nameof(VariableKey.CurrentTempMultiplier), value);
        }

        /// <summary>
        /// [제안] 한 번의 스와이프로 얻은 역대 최고 점수 (소위 '대박' 기록)
        /// </summary>
        public int BestSingleSwipeScore
        {
            get => Variables.GetVariable(nameof(VariableKey.BestSingleSwipeScore)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.BestSingleSwipeScore), value);
        }

        /// <summary>
        /// [제안] 한 번의 움직임에 발생한 최대 연쇄(콤보) 횟수
        /// </summary>
        public int MaxComboCount
        {
            get => Variables.GetVariable(nameof(VariableKey.MaxComboCount)).IntValue;
            set => Variables.SetInteger(nameof(VariableKey.MaxComboCount), value);
        }
        #endregion

        // --- Current prefix (실시간 값) ---

        public StageInfo CurrentStageInfo { get; set; }
        public int CurrentRerollRemain { get; set; }

        // public StageModel CurrentStageTarget { get; set; }


        /// <summary>
        /// 특정 접두사(Best, Total, Current 등)를 가진 변수들만 초기화합니다.
        /// </summary>
        public void ResetByPrefix(string prefix)
        {
            foreach (VariableKey key in Enum.GetValues(typeof(VariableKey)))
            {
                // 키 이름이 해당 prefix로 시작하는지 확인 (예: "Current")
                if (key.ToString().StartsWith(prefix))
                {
                    // Multiplier는 1로, 나머지는 0으로 초기화
                    if (key == VariableKey.CurrentTempMultiplier)
                        Variables.SetFloat(key.ToString(), 1.0f);
                    else
                        Variables.SetInteger(key.ToString(), 0);
                }
            }
        }

        /// <summary>
        /// 모든 변수 초기화
        /// </summary>
        public void ResetAll()
        {
            Variables = new VariableContainer();
            foreach (VariableKey key in Enum.GetValues(typeof(VariableKey)))
            {
                if (key == VariableKey.CurrentTempMultiplier)
                    Variables.SetFloat(key.ToString(), 1.0f);
                else
                    Variables.SetInteger(key.ToString(), 0);
            }
            CurrentStageInfo = null;
        }
        

        public PlayerState()
        {
           
        }

        public VariableContainer.Variable this[string key]
        {
            get
            {

                if (Variables.Items.ContainsKey(key))
                {
                    return Variables.Items[key];
                }

                return null;
            }
        }

        public void SaveVariable(string key, int value)
        {
            Variables.SetInteger(key, value);
        }

        public void SaveVariable(string key, float value)
        {
            Variables.SetFloat(key, value);
        }

        public PlayerState Clone()
        {
            var clone = new PlayerState();
            this.CopyTo(clone);
            return clone;
        }

        public void CopyTo(PlayerState target)
        {
            // target.CurrentScore = this.CurrentScore;
            // target.TotalScore = this.TotalScore;

            target.Variables = this.Variables.Clone();
        }

        public void CopyFrom(PlayerState source)
        {
            source.CopyTo(this);
        }

        [ConsoleCommand("Status", "플레이어 상태 정보를 출력합니다.")]
        public static void PrintStats()
        {
            McConsole.MessageSuccess("-- PlayerStatus-- ");
            foreach (var kvp in Current.Variables.Items)
            {
                McConsole.MessageInfo($"- {kvp.Key}: {kvp.Value.IntValue} (Float: {kvp.Value.FloatValue})");
            }
            McConsole.MessageSuccess("-- End of PlayerStatus --");
        }
        

        [ConsoleCommandClass]
        public class ModifyVariableCommand : IConsoleCommand
        {
            public string Command => "modify";
            public string Description => "플레이어의 변수를 수정합니다. 기본 동작은 set입니다.";
            public string Signature => "modify <VariableKey> <value> [set(default)|add|subtract]";

            public void Execute(string[] args)
            {
                if (args.Length < 2)
                {
                    McConsole.MessageError("Usage: " + Signature);
                    return;
                }

                string variableKey = args[0];
                if (!Enum.TryParse<PlayerState.VariableKey>(variableKey, out var key))
                {
                    McConsole.MessageError($"Invalid VariableKey: {variableKey}");
                    return;
                }

                if (!int.TryParse(args[1], out int value))
                {
                    McConsole.MessageError($"Invalid value: {args[1]}");
                    return;
                }

                int behavior = 0; // 0: set, 1: add, 2: subtract
                if (args.Length >= 3)
                {
                    switch (args[2].ToLower())
                    {
                        case "set":
                            behavior = 0;
                            break;
                        case "add":
                            behavior = 1;
                            break;
                        case "subtract":
                            behavior = 2;
                            break;
                        default:
                            McConsole.MessageError($"Invalid behavior: {args[2]}");
                            return;
                    }
                }

                void Set(int v)
                {
                    PlayerState.Current.SaveVariable(key.ToString(), v);
                }

                void Add(int v)
                {
                    int current = Current.Variables.GetVariable(key.ToString()).IntValue;

                    Set(current + v);
                }

                void Subtract(int v)
                {
                    int current = Current.Variables.GetVariable(key.ToString()).IntValue;

                    Set(current - v);
                }

                switch (behavior)
                {
                    case 0:
                        Set(value);
                        McConsole.MessageSuccess($"Set {variableKey} to {value}");
                        break;
                    case 1:
                        Add(value);
                        McConsole.MessageSuccess($"Added {value} to {variableKey}");
                        break;
                    case 2:
                        Subtract(value);
                        McConsole.MessageSuccess($"Subtracted {value} from {variableKey}");
                        break;
                }
            }

            public void AutoComplete(Span<string> args, ref List<string> suggestions)
            {
                if (args.Length == 1)
                {
                    foreach (var name in Enum.GetNames(typeof(PlayerState.VariableKey)))
                    {
                        if (name.StartsWith(args[0], StringComparison.OrdinalIgnoreCase))
                        {
                            suggestions.Add(name);
                        }
                    }
                }
                else if (args.Length == 2)
                {
                    // Suggest some example values
                    suggestions.Add("0");
                    suggestions.Add("10");
                    suggestions.Add("100");
                    suggestions.Add("1000");
                    suggestions.Add("10000");
                }
                else if (args.Length == 3)
                {
                    string[] behaviors = { "set", "add", "subtract" };
                    foreach (var behavior in behaviors)
                    {
                        if (behavior.StartsWith(args[2], StringComparison.OrdinalIgnoreCase))
                        {
                            suggestions.Add(behavior);
                        }
                    }
                }
            }
        }

        public void SetUpNewGame()
        {
            Reset();
            gameDeck.Clear();
            reinforcementContainer.Initialize();
            CurrentStageInfo = StageLibrary.Instance.GetStageInfo(1);
            foreach (var item in IngredientLibrary.Instance.AllIngredientList)
            {
                gameDeck.SetIngredientCount(item, item.startAmount);
            }
        }

        public int GetKimbapScore()
        {
            return CurrentTempScore - CurrentStageScore;
        }
    }
    

    // public class CurrentCoinChangedEventArgs : ExecEventArgs<CurrentCoinChangedEventArgs>
    // {
    //     public int OldCurrentCoin { get; set; }
    //     public int NewCurrentCoin { get; set; }
    // }
    
}
