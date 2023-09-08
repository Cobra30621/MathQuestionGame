using System.Collections.Generic;
using Newtonsoft.Json;
using NueGames.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyAbility
{
    /// <summary>
    /// Represents an enemy's abilities.
    /// </summary>
    public class EnemyAbilityData
    {
        /// <summary>
        /// Indicates whether to use the start battle skillData.
        /// </summary>
        public bool UseStartBattleSkill;

        /// <summary>
        /// The start battle skillData.
        /// </summary>
        [ShowIf("UseStartBattleSkill")]
        public EnemySkillData StartBattleSkillData;
        
        /// <summary>
        /// List of enemy skills.
        /// </summary>
        [TableList(AlwaysExpanded = true)]
        public List<EnemySkillData> enemySkills;

        
        
    }
}
