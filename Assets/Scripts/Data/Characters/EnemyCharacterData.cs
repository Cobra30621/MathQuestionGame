using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.NueExtentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Data.Characters
{
    [CreateAssetMenu(fileName = "Enemy Character Data",menuName = "NueDeck/Characters/Enemy",order = 1)]
    public class EnemyCharacterData : CharacterDataBase
    {
        [Header("Enemy Defaults")] 
        [SerializeField] private EnemyBase enemyPrefab;
        [SerializeField] private bool followAbilityPattern;
        [SerializeField] private List<EnemyAbilityData> enemyAbilityList;
        public List<EnemyAbilityData> EnemyAbilityList => enemyAbilityList;

        public EnemyBase EnemyPrefab => enemyPrefab;

        public EnemyAbilityData GetAbility()
        {
            return EnemyAbilityList.RandomItem();
        }
        
        public EnemyAbilityData GetAbility(int usedAbilityCount)
        {
            if (followAbilityPattern)
            {
                var index = usedAbilityCount % EnemyAbilityList.Count;
                return EnemyAbilityList[index];
            }

            return GetAbility();
        }
    }
    
    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private string name;
        [SerializeField] private EnemyIntentionData intention;
        [SerializeField] private bool hideActionValue;
        [SerializeField] private List<EnemyActionData> actionList;
        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<EnemyActionData> ActionList => actionList;
        public bool HideActionValue => hideActionValue;
    }
    
    [Serializable]
    public class EnemyActionData
    {
        [SerializeField] private EnemyActionType actionType;
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private int minActionValue;
        [SerializeField] private int maxActionValue;
        [Header("給予狀態（只有 actionType 選擇 Give Status，才需要選擇）")]
        [SerializeField] private PowerType powerType;
        public EnemyActionType ActionType => actionType;
        public ActionTargetType ActionTargetType => actionTargetType;
        public PowerType PowerType => powerType;
        public int ActionValue => Random.Range(minActionValue,maxActionValue);
        
    }
    
    
    
}