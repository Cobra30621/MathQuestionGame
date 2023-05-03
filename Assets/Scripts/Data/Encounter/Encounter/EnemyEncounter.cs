using System;
using System.Collections.Generic;
using NueGames.Data.Characters;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    /// <summary>
    /// 遭遇一場戰鬥的敵人清單
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Enemy Encounter", menuName = "NueDeck/Encounter/EnemyEncounter")]
    public class EnemyEncounter : EncounterBase
    {
        public EnemyEncounterName encounterName;
        public List<EnemyCharacterData> enemyList;
    }

    /// <summary>
    /// 敵人遭遇名稱
    /// </summary>
    /// TODO 撰寫單元測試檢查
    public enum EnemyEncounterName
    {
        OneSmile = 0,
        TwoSmiles = 1,
        BigSmile = 2,
        SmileGroup = 3
    }
    
    
}