using System;
using System.Collections;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common;
using Common.Attributes;
using Database.Generated;
using DG.Tweening;
using Game.Field;
using Machamy.Attributes;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public float baseScore = 10f;
        public int startAmount = 0;
        public float variable01 = 0f;
        public float variable02 = 0f;
        public List<string> additionalVariables = new List<string>();
        
        
        public Color debugColor = Color.white;
        public abstract IEnumerator OnFall(Tile tile);


        public virtual IEnumerator OnTrigger(Tile tile)
        {
            PlayerState.Current.CurrentTempScore += (int) baseScore;
            yield return tile.GetComponent<SpriteRenderer>().DOColor(Color.yellow, 0.2f).OnComplete(() =>
            {
                tile.GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.2f);
            });
            yield break;
        }
        public virtual IEnumerator OnExplode(Tile tile)
        {
            tile.SetIngredient(null);
            PuzzleManager.Instance.RetrieveIngredient(this);
            yield return tile.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.2f).OnComplete(() =>
            {
                tile.GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.2f);
            });
            yield break;
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
            baseScore = data.baseScore;
            startAmount = data.startAmount;
            variable01 = data.variable01;
            variable02 = data.variable02;
            
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