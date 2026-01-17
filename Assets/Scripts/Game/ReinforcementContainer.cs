using System.Collections.Generic;
using BandoWare.GameplayTags;
using Common.Collections;
using Common.Singleton;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class ReinforcementContainer
    {
        private SerializableDictionary<GameplayTag, int> reinforcementValues = new ();

        public void Initialize()
        {
            reinforcementValues.Clear();
        }
        
        public void AddReinforcement(GameplayTag tag, int value)
        {
            if (reinforcementValues.ContainsKey(tag))
            {
                reinforcementValues[tag] += value;
            }
            else
            {
                reinforcementValues[tag] = value;
            }
        }
        
        public int ProcessBaseValue(GameplayTag tag, int baseValue)
        {
            foreach (var kvp in reinforcementValues)
            {
                if (tag.IsParentOf(kvp.Key) || kvp.Key == tag)
                {
                    baseValue += kvp.Value;
                }
            }
            return baseValue;
        }
    }
}