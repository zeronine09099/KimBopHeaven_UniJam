
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BandoWare.GameplayTags;
using Common.Collections;
using Common.Singleton;
using Database;
using Machamy.Attributes;
using Machamy.Utils;
using UnityEngine;


namespace Game
{
    /// <summary>
    /// 재료 관리를 담당하는 매니저 클래스
    /// </summary>
    public class IngredientManager : Singleton<IngredientManager>
    {
        [field:SerializeField] public bool IsInitialized { get; private set; } = false;

        [SerializeField,VisibleOnly] private SerializableDictionary<GameplayTag, IngredientSO> ingredientDictionary = new();

        private List<GameplayTag> cachedIngredientTagList = null;
        List<IngredientSO> cachedIngredientList = null;
        
        public IReadOnlyDictionary<GameplayTag, IngredientSO> AllIngredients => ingredientDictionary;
        public IReadOnlyList<GameplayTag> AllIngredientTagList => cachedIngredientTagList;
        public IReadOnlyList<IngredientSO> AllIngredientList => cachedIngredientList;
        
        protected override void AfterAwake()
        {
            
        }
        
        public IEnumerator Init(System.Action onCompleted = null)
        {
            yield return LoadAndInitializeIngredients();
            cachedIngredientTagList = ingredientDictionary.Keys.ToList();
            cachedIngredientList = ingredientDictionary.Values.ToList();
            IsInitialized = true;
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
        
        /// <summary>
        /// 태그에 해당하는 IngredientSO를 반환합니다.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IngredientSO GetIngredientSO(GameplayTag tag)
        {
            if (ingredientDictionary.TryGetValue(tag, out var ingredientSO))
            {
                return ingredientSO;
            }
            LogEx.LogError($"IngredientSO not found for tag: {tag}");
            return null;
        }
        

    }
}