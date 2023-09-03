using System;
using System.Collections.Generic;
using EnemyAbility;
using NueGames.Data.Characters;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    /// <summary>
    /// 遭遇一場戰鬥的敵人清單
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Enemy Encounter", menuName = "Enemy/EnemyEncounter")]
    public class EnemyEncounter : EncounterBase
    {
        [InlineEditor()]
        public List<EnemyData> enemyList;
    }
}