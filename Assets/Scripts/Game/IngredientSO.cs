using System;
using System.Collections;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common;
using Common.Attributes;
using Cysharp.Threading.Tasks;
using Database.Generated;
using DG.Tweening;
using Game.Field;
using Machamy.Attributes;
using Machamy.Utils;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

namespace Game
{
    public abstract class IngredientSO : ScriptableObject
    {

        public abstract GameplayTag Tag { get; }

        public int order;
        #if UNITY_EDITOR
        [SerializeField, VisibleOnly,Label("설정된 태그")] private string debugTag;
        #endif
        
        public Sprite icon;
        public Rarity rarity;
        public string displayName;
        public string description;
        public int baseScore;
        public int ReinforcedBaseScore => PlayerState.Current.Reinforcements.ProcessBaseValue(Tag, baseScore);
        public int startAmount = 0;
        public float variable01 = 0f;
        public float variable02 = 0f;
        public List<string> additionalVariables = new List<string>();
        
        
        public float DefaultTriggerInterval = 0.2f;
        
        public Color debugColor = Color.white;

        public async virtual UniTask OnFall(Tile tile)
        {
            await UniTask.CompletedTask;
        }

        protected Tween DefaultTriggerEffect(TriggerArguments args, int times, int score = -1)
        {
            var obj = args.Ingredient;
            float[] punchScales = {1.05f, 1.1f, 1.15f, 1.2f, 1.25f};
            float[] fontSizes = {72f,80f,88f,96f,104f};
            float punchScale = punchScales[Mathf.Clamp(times - 1, 0, punchScales.Length - 1)];
            if(score > 0)
            {
                var tile = args.Tile;
                var scoreText = tile.scoreText;
                if (scoreText == null)
                {
                    LogEx.LogError($"Score Text가 널");
                }
                else
                {
                    if (scoreText.IsFading || !scoreText.gameObject.activeSelf)
                    {
                        scoreText.Show();
                    }
                    scoreText.Value = score;
                    scoreText.FontSize = fontSizes[Mathf.Clamp(times - 1, 0, fontSizes.Length - 1)];
                }
            }
            Sequence seq = DOTween.Sequence();
            seq.Append(obj.transform.DOScale(Vector3.one * punchScale, 0.1f).SetEase(Ease.OutQuad));
            seq.Append(obj.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutQuad));

            return seq;
        }
        
        public static void AddIngredientToPlayer(IngredientSO ingredient, int amount)
        {
            if (ingredient == null) return;
            PlayerState.Current.GameDeck.AddIngredient(ingredient, amount);
            PuzzleManager.Instance.AddIngredientToRetrieved(ingredient, amount);
        }

        
        public async virtual UniTask OnTrigger(TriggerArguments args)
        {
            var tile = args.Tile;
            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            
            DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));

        }
        public async virtual UniTask OnExplode(Tile tile)
        {
            await UniTask.CompletedTask;
        }


        public void OnEnable()
        {
            #if UNITY_EDITOR
            debugTag = Tag.ToString();
            #endif
        }

        public void OnValidate()
        {
            #if UNITY_EDITOR
            debugTag = Tag.ToString();
            #endif
        }
        
        
        public virtual void InitByData(IngredientData data)
        {
            // icon = data.icon;
            displayName = data.koreanName;
            description = data.description;
            rarity = data.rarity;
            baseScore = (int) data.baseScore;
            startAmount = data.startAmount;
            variable01 = data.variable01;
            variable02 = data.variable02;
            icon = Resources.Load<Sprite>("Sprites/Ingredients/" + data.name);
            
            var savedState = UnityEngine.Random.state;
            UnityEngine.Random.InitState(Tag.GetHashCode());
            debugColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            UnityEngine.Random.state = savedState;
            if (data.additionalVariables != null)
            {
                additionalVariables = new List<string>(data.additionalVariables);
            }
        }

        public static explicit operator IngredientSO(GameplayTag tag)
        {
            return IngredientLibrary.Instance.GetIngredientSO(tag);
        }
    }
    
    
    public static class IngredientSOExtensions
    {
        public static bool IsGim(this IngredientSO ingredient)
        {
            return ingredient != null && ingredient.Tag == AllGameplayTags.Ingredient.Essential.Gim.Get();
        }
        
        public static bool IsRice(this IngredientSO ingredient)
        {
            return ingredient != null && ingredient.Tag == AllGameplayTags.Ingredient.Essential.Rice.Get();
        }
    }
}