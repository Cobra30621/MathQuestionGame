using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyPrefabData : ScriptableObject
    {
        public List<EnemyBase> prefabs;


        public EnemyBase GetPrefab(string name)
        {
            var first = prefabs.First(p => p.name == name);

            if (first == null)
            {
                Debug.LogError($"No enemy prefab found with the name '{name}'.");
                return null;
            }
            
            return first;
        }
    }
}