using System;
using System.Collections.Generic;
using Common.Collections;
using Common.Extentions;
using Game;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] SerializableDictionary<IngredientSO, int> ingredientCountMap = new SerializableDictionary<IngredientSO, int>();
        // public IReadOnlyList<IngredientSO> IngredientList => ingredientList;

        public Inventory()
        {
            if (ingredientCountMap == null)
            {
                ingredientCountMap = new SerializableDictionary<IngredientSO, int>();
            }
        }

        public void Initialize()
        {
            ingredientCountMap.Clear();
            foreach (var ingredient in IngredientLibrary.Instance.AllIngredientList)
            {
                ingredientCountMap[ingredient] = 0;
            }
        }
        
        public void AddIngredient(IngredientSO ingredient)
        {
            if (ingredientCountMap.TryGetValue(ingredient, out var count))
            {
                ingredientCountMap[ingredient] = count + 1;
            }
            else
            {
                ingredientCountMap[ingredient] = 1;
            }
        }
        
        public void RemoveIngredient(IngredientSO ingredient)
        {
            if (ingredientCountMap.TryGetValue(ingredient, out var count) && count > 0)
            {
                ingredientCountMap[ingredient] = count - 1;
            }
        }
        
        public void SetIngredientCount(IngredientSO ingredient, int count)
        {
            ingredientCountMap[ingredient] = count;
        }
        
        public bool ContainsIngredient(IngredientSO ingredient)
        {
            return ingredientCountMap.ContainsKey(ingredient) && ingredientCountMap[ingredient] > 0;
        }
        
        public int GetIngredientCount(IngredientSO ingredient)
        {
            if (ingredientCountMap.TryGetValue(ingredient, out var count))
            {
                return count;
            }
            return 0;
        }
        
        public IEnumerator<IngredientSO> GetShuffledEnumerator()
        {
            using var handle = ListPool<IngredientSO>.Get(out var shuffledList);
            foreach (var kvp in ingredientCountMap)
            {
                if (kvp.Value > 0)
                {
                    shuffledList.Add(kvp.Key);
                }
            }
            shuffledList.Shuffle();
            foreach (var ingredient in shuffledList)
            {
                yield return ingredient;
            }
        }
        
        public void Clear()
        {
            ingredientCountMap.Clear();
        }
    }
}