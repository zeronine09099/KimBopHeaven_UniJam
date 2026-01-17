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
        
        /// <summary>
        /// 게임 시작 시 초기화되는 인벤토리(덱)입니다.
        /// </summary>
        public Inventory GameDeck => gameDeck;
        
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
                Variables.SetInteger(key.ToString(), 0);
            }
            CurrentStageInfo = null;
        }

        public enum VariableKey
        {
            BestScore, // 1번의 스와이프로 얻은 최고 점수
            BestStageScore, // 해당 게임 중 기록된 최고 스테이지 점수
            BestStageKimbapCount, // 해당 게임에서 기록한 최고 김밥 횟수
            
            TotalScore, // 해당 게임 진행 중 누적된 총 점수
            TotalClearedElements, // 해당 게임 진행 중 누적된 총 지운 수
            TotalSwipe , // 해당 게임 진행 중 누적된 총 스와이프 횟수
            TotalKimbapCount, // 해당 게임 진행 중 누적된 김밥 횟수
            
            CurrentRemainingSwipes, // 현재 남은 스와이프 횟수
            CurrentStageScore, // 현재 스테이지 점수
            CurrentTempScore, // 현재 임시 점수 (스와이프 중간에 변동되는 점수)
            CurrentTempMultiplier, // 현재 임시 멀티플라이어 (스와이프 중간에 변동되는 멀티플라이어)
            
        }
        
        // for 문 사용 편의성
        public static readonly VariableKey BestStart = VariableKey.BestScore;
        public static readonly VariableKey BestEnd = VariableKey.BestStageKimbapCount;
        public static readonly VariableKey TotalStart = VariableKey.TotalScore;
        public static readonly VariableKey TotalEnd = VariableKey.TotalKimbapCount;
        // public static readonly VariableKey StageStart = VariableKey.StageBestPlacement;
        // public static readonly VariableKey StageEnd = VariableKey.StageTempScore;
        public static readonly VariableKey CurrentStart = VariableKey.CurrentRemainingSwipes;
        public static readonly VariableKey CurrentEnd = VariableKey.CurrentTempScore;
        
        
        // --- PlayerStatus Properties (VariableKey에 매핑된 프로퍼티들) ---

        // --- Best prefix ---

        /// <summary>
        /// 1번의 스와이프로 얻은 최고 점수입니다. (Prefix: Best)
        /// </summary>
        public int BestScore
        {
            get { return Variables.GetVariable(nameof(VariableKey.BestScore)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.BestScore), value); }
        }

        /// <summary>
        /// 해당 게임 중 기록된 최고 스테이지 점수입니다. (Prefix: BestStage)
        /// </summary>
        public int BestStageScore
        {
            get { return Variables.GetVariable(nameof(VariableKey.BestStageScore)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.BestStageScore), value); }
        }

        /// <summary>
        /// 해당 게임에서 기록한 최고 김밥 횟수입니다. (Prefix: BestStage)
        /// </summary>
        public int BestStageKimbapCount
        {
            get { return Variables.GetVariable(nameof(VariableKey.BestStageKimbapCount)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.BestStageKimbapCount), value); }
        }

        // --- Total prefix ---

        /// <summary>
        /// 해당 게임 진행 중 누적된 총 점수입니다. (Prefix: Total)
        /// </summary>
        public int TotalScore
        {
            get { return Variables.GetVariable(nameof(VariableKey.TotalScore)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.TotalScore), value); }
        }

        /// <summary>
        /// 해당 게임 진행 중 누적된 총 지운 수입니다. (Prefix: Total)
        /// </summary>
        public int TotalClearedElements
        {
            get { return Variables.GetVariable(nameof(VariableKey.TotalClearedElements)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.TotalClearedElements), value); }
        }

        /// <summary>
        /// 해당 게임 진행 중 누적된 총 스와이프 횟수입니다. (Prefix: Total)
        /// </summary>
        public int TotalSwipe
        {
            get { return Variables.GetVariable(nameof(VariableKey.TotalSwipe)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.TotalSwipe), value); }
        }

        /// <summary>
        /// 해당 게임 진행 중 누적된 김밥 횟수입니다. (Prefix: Total)
        /// </summary>
        public int TotalKimbapCount
        {
            get { return Variables.GetVariable(nameof(VariableKey.TotalKimbapCount)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.TotalKimbapCount), value); }
        }

        // --- Current prefix ---

        /// <summary>
        /// 현재 남은 스와이프 횟수입니다. (Prefix: Current)
        /// </summary>
        public int CurrentRemainingSwipes
        {
            get { return Variables.GetVariable(nameof(VariableKey.CurrentRemainingSwipes)).IntValue; }
            set { Variables.SetInteger(nameof(VariableKey.CurrentRemainingSwipes), value); }
        }

        /// <summary>
        /// 현재 스테이지 점수입니다. (Prefix: Current)
        /// </summary>
        public int CurrentStageScore
        {
            get { return Variables.GetVariable(nameof(VariableKey.CurrentStageScore)).IntValue; }
            set
            {
                Variables.SetInteger(nameof(VariableKey.CurrentStageScore), value);
                UIManager.Instance.InGameUI.ScoreUI.CurrentValue = value;
            }
        }
        
        /// <summary>
        /// 현재 임시 점수입니다. (Prefix: Current)
        /// </summary>
        public int CurrentTempScore
        {
            get { return Variables.GetVariable(nameof(VariableKey.CurrentTempScore)).IntValue; }
            set
            {
                bool isIncreased = value > CurrentTempScore;
                Variables.SetInteger(nameof(VariableKey.CurrentTempScore), value);
                UIManager.Instance.InGameUI.ScoreUI.TempValue = value;
                if (isIncreased)
                {
                    SoundManager.Instance.PlayScoreSfx();
                }
            }
        }
        
        public float CurrentTempMultiplier
        {
            get { return Variables.GetVariable(nameof(VariableKey.CurrentTempMultiplier)).FloatValue; }
            set
            {
                Variables.SetFloat(nameof(VariableKey.CurrentTempMultiplier), value);
            }
        }

        // --- Current prefix (실시간 값) ---

        public StageInfo CurrentStageInfo { get; set; }
        public int CurrentRerollRemain { get; set; }

        // public StageModel CurrentStageTarget { get; set; }


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
