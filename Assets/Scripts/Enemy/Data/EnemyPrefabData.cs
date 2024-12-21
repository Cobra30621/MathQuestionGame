using System.Collections.Generic;
using System.Linq;
using NSubstitute.Core;
using UnityEngine;

namespace Enemy
{
    public class EnemyPrefabData : ScriptableObject
    {
        public List<Enemy> prefabs;


        public Enemy GetPrefab(string name, string whoFinding = "")
        {
            var first = prefabs.FirstOrDefault(p => p.name == name);

            if (first == null)
            {
                Debug.LogError($"{whoFinding} can't find enemy prefab with the name '{name}'.");
                return null;
            }
            
            return first;
        }
    }
}