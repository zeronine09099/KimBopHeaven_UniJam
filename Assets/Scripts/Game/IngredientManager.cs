
using System.Collections;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common.Collections;
using Common.Singleton;
using Database;
using Machamy.Utils;
using UnityEngine;


namespace Game
{
    /// <summary>
    /// 재료 관리를 담당하는 매니저 클래스
    /// </summary>
    public class IngredientManager : Singleton<IngredientManager>
    {
        [SerializeField] private SerializableDictionary<GameplayTag, IngredientSO> ingredientDictionary = new();
        
        protected override void AfterAwake()
        {
            
        }
        
        public IEnumerator Init(System.Action onCompleted = null)
        {
            yield return LoadAndInitializeIngredients();
            onCompleted?.Invoke();
        }
        private IEnumerator LoadAndInitializeIngredients()
        {
            // SO 등록
            var ingredientSOList = Resources.LoadAll<IngredientSO>("ScriptableObjects/Ingredients");
            foreach (var ingredientSO in ingredientSOList)
            {
                ingredientDictionary.Add(ingredientSO.Tag, ingredientSO);
            }
            
            // DB매니저에서 초기화
            yield return new WaitWhile(() => !DatabaseManager.Instance.IsInitialized);

            var ingredientDBEntries = DatabaseManager.Instance.Database.IngredientDataList;
            foreach (var entry in ingredientDBEntries)
            {
                GameplayTag tag = GameplayTagManager.RequestTag(entry.tag);
                if (ingredientDictionary.ContainsKey(tag))
                {
                    ingredientDictionary[tag].InitByData(entry);
                }
                else
                {
                    LogEx.LogError($"IngredientSO not found for tag: {entry.tag}");
                }
            }
            
            
            yield break;
        }
    }
}