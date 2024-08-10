using System;
using System.Collections.Generic;
using Enemy;
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
        public List<EnemyName> enemyList;
    }
}