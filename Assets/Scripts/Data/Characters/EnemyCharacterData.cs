using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.NueExtentions;
using Sirenix.Serialization;
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
        // [SerializeField] private EnemyAbilityData 
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
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private List<ActionData> actionList;
        [SerializeField] private ActionDataClip actionDataClip;
        
        
        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<ActionData> ActionList => actionList;
        public ActionTargetType ActionTargetType => actionTargetType;
        public bool HideActionValue => hideActionValue;
        public ActionDataClip ActionDataClip => actionDataClip;
    }
}