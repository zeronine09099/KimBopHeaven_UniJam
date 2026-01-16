
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BandoWare.GameplayTags;
using Common;
using Common.Collections;
using Common.Singleton;
using Cysharp.Threading.Tasks;
using Database;
using Machamy.Attributes;
using Machamy.Utils;
using UnityEngine;


namespace Game
{
    /// <summary>
    /// 재료 관리를 담당하는 클래스
    /// </summary>
    public class IngredientLibrary : Singleton<IngredientLibrary>
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
        
        public async UniTask Init(System.Action onCompleted = null)
        {
            LogEx.Log("Initializing IngredientLibrary...");
            ingredientDictionary.Clear();
            await LoadAndInitializeIngredients();
            var pairs = ingredientDictionary.ToList();
            pairs.Sort((a, b) => a.Value.order.CompareTo(b.Value.order));
            cachedIngredientTagList = pairs.Select(pair => pair.Key).ToList();
            cachedIngredientList = pairs.Select(pair => pair.Value).ToList();
            IsInitialized = true;
            LogEx.Log("IngredientLibrary initialized.");
            onCompleted?.Invoke();
        }
        private async UniTask LoadAndInitializeIngredients()
        {
            // SO 등록
            var ingredientSOList = Resources.LoadAll<IngredientSO>("ScriptableObjects/Ingredients");
            foreach (var ingredientSO in ingredientSOList)
            {
                if (!ingredientDictionary.ContainsKey(ingredientSO.Tag))
                {
                    ingredientDictionary.Add(ingredientSO.Tag, ingredientSO);
                }
                else
                {
                    LogEx.LogError($"Duplicate IngredientSO for tag: {ingredientSO.Tag} ({ingredientSO.name}");
                }
            }
            
            // DB매니저에서 초기화
            // yield return new WaitWhile(() => !DatabaseManager.Instance.IsInitialized);
            await new WaitUntil(() => DatabaseManager.Instance.IsInitialized);

            var ingredientDBEntries = DatabaseManager.Instance.Database.IngredientDataList;
            int order = 0;
            foreach (var entry in ingredientDBEntries)
            {
                GameplayTag tag = GameplayTagManager.RequestTag(entry.tag);
                if (ingredientDictionary.ContainsKey(tag))
                {
                    ingredientDictionary[tag].InitByData(entry);
                    ingredientDictionary[tag].order = order;
                }
                else
                {
                    LogEx.LogError($"IngredientSO not found for tag: {entry.tag}");
                }
                order++;
            }
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
        
        
        public IngredientSO GetRandomIngredientSO()
        {
            int randomIndex = Random.Range(0, cachedIngredientList.Count);
            return cachedIngredientList[randomIndex];
        }
        
        public IngredientSO GetRandomIngredientSOByRarity(Rarity rarity)
        {
            var filteredList = cachedIngredientList.Where(ing => ing.rarity == rarity).ToList();
            if (filteredList.Count == 0)
            {
                LogEx.LogWarning($"No ingredients found with rarity: {rarity}. Returning a random ingredient instead.");
                return GetRandomIngredientSO();
            }
            int randomIndex = Random.Range(0, filteredList.Count);
            return filteredList[randomIndex];
        }

    }
}