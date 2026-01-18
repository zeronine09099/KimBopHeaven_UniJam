using System;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common.Collections;
using Common.Extentions;
using Game;
using Machamy.DeveloperConsole.Attributes;
using Machamy.DeveloperConsole.Commands;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] SerializableDictionary<IngredientSO, int> ingredientCountMap = new SerializableDictionary<IngredientSO, int>();
        // public IReadOnlyList<IngredientSO> IngredientList => ingredientList;
        
        [SerializeField] IngredientSO airIngredientSO;

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

            airIngredientSO = IngredientLibrary.Instance.GetIngredientSO(AllGameplayTags.Ingredient.Etc.Air.Get());
        }
        
        public void AddIngredient(IngredientSO ingredient, int count = 1)
        {
            if (ingredientCountMap.ContainsKey(airIngredientSO) && ingredientCountMap[airIngredientSO] > 0)
            {
                ingredientCountMap[airIngredientSO] -= 1;
            }
            if (ingredientCountMap.TryGetValue(ingredient, out var existingCount))
            {
                ingredientCountMap[ingredient] = existingCount + count;
            }
            else
            {
                ingredientCountMap[ingredient] = count;
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
        
        public IEnumerable<IngredientSO> GetShuffledSOEnumerator()
        {
            using var handle = ListPool<IngredientSO>.Get(out var shuffledList);
            foreach (var kvp in ingredientCountMap)
            {
                int count = kvp.Value;
                for (int i = 0; i < count; i++)
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
        
        public IEnumerable<IngredientSO> GetSOEnumerator()
        {
            foreach (var kvp in ingredientCountMap)
            {
                int count = kvp.Value;
                for (int i = 0; i < count; i++)
                {
                    yield return kvp.Key;
                }
            }
        }
        
        public void Clear()
        {
            ingredientCountMap.Clear();
        }
        
        public Inventory Clone()
        {
            var newInventory = new Inventory();
            foreach (var kvp in ingredientCountMap)
            {
                newInventory.ingredientCountMap[kvp.Key] = kvp.Value;
            }
            return newInventory;
        }
    }



    
}