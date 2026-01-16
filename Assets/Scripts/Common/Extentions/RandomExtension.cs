using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Extentions
{
    public static class RandomExtension
    {

        public static void Shuffle<T>(this IList<T> values)
        {
            for (int i = values.Count - 1; i > 0; i--)
            {
                int k = Random.Range(0, i + 1);
                (values[k], values[i]) = (values[i], values[k]);
            }
        }
        
    }
}