using System;
using System.Collections.Generic;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    public class EncounterGenerator : MonoBehaviour
    {
        [SerializeField] private List<EnemyEncounter> EnemyEncounters;
        public Dictionary<EnemyEncounterName, EnemyEncounter> EnemyEncounterDict;


        private void Awake()
        {
            SetUp();
        }


        public void SetUp()
        {
            EnemyEncounterDict = new Dictionary<EnemyEncounterName, EnemyEncounter>();
            foreach (var enemyEncounter in EnemyEncounters)
            {
                EnemyEncounterName encounterName = enemyEncounter.encounterName;
                if (EnemyEncounterDict.ContainsKey(encounterName))
                {
                    Debug.LogError($"{nameof(EnemyEncounterDict)} 的 {encounterName} 重複了");
                }
                else
                {
                    EnemyEncounterDict.Add(encounterName, enemyEncounter);
                }
            }
        }

        public EnemyEncounter GetEnemyEncounter(EnemyEncounterName encounterName)
        {
            if (EnemyEncounterDict.ContainsKey(encounterName))
            {
                return EnemyEncounterDict[encounterName];
            }
            else
            {
                Debug.LogError($"{nameof(EnemyEncounterDict)} 沒有 {encounterName} 的 {nameof(encounterName)}");
                return null;
            }
        }
    }
}