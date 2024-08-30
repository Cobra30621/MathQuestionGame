using System;
using System.Collections.Generic;
using Enemy;
using NueGames.Enums;
using rStarTools.Scripts.StringList;
using UnityEngine;

namespace Data.Encounter
{
    /// <summary>
    /// 遭遇一場戰鬥的敵人清單
    /// </summary>
    [Serializable]
    public class EnemyEncounter : DataBase<EnemyEncounterOverview>
    {
        public List<EnemyName> enemyList;
        
        [SerializeField] private BackgroundTypes targetBackgroundType;
        
        public BackgroundTypes TargetBackgroundType => targetBackgroundType;

        
        
    }
}