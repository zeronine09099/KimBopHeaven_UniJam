using System;
using System.Collections;
using BandoWare.GameplayTags;
using Common.Attributes;
using Database.Generated;
using Game.Field;
using Machamy.Attributes;
using UnityEngine;

namespace Game
{
    public abstract class IngredientSO : ScriptableObject
    {

        public abstract GameplayTag Tag { get; }

        #if UNITY_EDITOR
        [SerializeField, VisibleOnly,Label("설정된 태그")] private string debugTag;
        #endif
        
        
        public Sprite icon;
        
        public string displayName;
        public string description;
        public float baseScore = 10f;
        public float scoreMultiplier = 1f;
        
        public abstract IEnumerator OnFall(Tile tile);
        public abstract IEnumerator OnExplode(Tile tile);


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
        
        
        public void InitByData(IngredientData data)
        {
            // icon = data.icon;
            displayName = data.koreanName;
            description = data.description;
            baseScore = data.baseScore;
            // scoreMultiplier = data.scoreMultiplier;
        }
    }
}