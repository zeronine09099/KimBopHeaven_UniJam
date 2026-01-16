using System;
using System.Collections;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common;
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
        public Rarity rarity;
        public string displayName;
        public string description;
        public float baseScore = 10f;
        public float variable01 = 0f;
        public float variable02 = 0f;
        public List<string> additionalVariables = new List<string>();
        
        public abstract IEnumerator OnFall(Tile tile);


        public abstract IEnumerator OnTrigger(Tile tile);
        public virtual IEnumerator OnExplode(Tile tile)
        {
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
            variable01 = data.variable01;
            variable02 = data.variable02;
            if (data.additionalVariables != null)
            {
                additionalVariables = new List<string>(data.additionalVariables);
            }
        }
    }
}