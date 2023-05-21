using Map;
using NueGames.Data.Encounter;
using UnityEngine;

namespace NueGames.Encounter
{
    using System.Collections.Generic;
    using System.Linq;

    public class WeightedRandom {
        /// <summary>
        /// 根據權重，隨機取的物件
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetWeightedRandomObjects<T>(List<T> objects, int count) where T : IWeightedObject {
            List<T> result = new List<T>();
            if (objects.Count == 0)
            {
                return result;
            }
            
            // 計算權重加總
            float totalWeight = objects.Sum(obj => obj.Weight);
            for (int i = 0; i < count; i++)
            {
                float randomWeight = Random.Range(0f, totalWeight);
                int currentWeight = 0;
                foreach (var t in objects)
                {
                    currentWeight += t.Weight;
                    if ( currentWeight >= randomWeight)
                    {
                        // Debug.Log($"random {randomWeight} , current {currentWeight}");
                        result.Add(t);
                        break;
                    }
                }
            }
            return result;
        }
    }
    
    public interface IWeightedObject {
        int Weight { get; }
    }
}