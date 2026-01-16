using System;
using System.Collections.Generic;
using Common.Extentions;
using Game;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] List<IngredientSO> ingredientList = new List<IngredientSO>();
        
        public IReadOnlyList<IngredientSO> IngredientList => ingredientList;
        
        public void AddIngredient(IngredientSO ingredient)
        {
            ingredientList.Add(ingredient);
        }
        
        public void RemoveIngredient(IngredientSO ingredient)
        {
            ingredientList.Remove(ingredient);
        }
        
        public bool ContainsIngredient(IngredientSO ingredient)
        {
            return ingredientList.Contains(ingredient);
        }
        
        public int GetIngredientCount(IngredientSO ingredient)
        {
            int count = 0;
            foreach (var item in ingredientList)
            {
                if (item == ingredient)
                {
                    count++;
                }
            }
            return count;
        }
        
        public IEnumerator<IngredientSO> GetShuffledEnumerator()
        {
            using var handle = ListPool<IngredientSO>.Get(out var shuffledList);
            shuffledList.AddRange(ingredientList);
            shuffledList.Shuffle();
            foreach (var ingredient in shuffledList)
            {
                yield return ingredient;
            }
        }
        
        public void Clear()
        {
            ingredientList.Clear();
        }
    }
}