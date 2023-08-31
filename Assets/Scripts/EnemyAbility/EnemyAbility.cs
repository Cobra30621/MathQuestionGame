using System.Collections.Generic;
using NueGames.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyAbility
{
    /// <summary>
    /// Represents an enemy's abilities.
    /// </summary>
    public class EnemyAbility
    {
        /// <summary>
        /// Indicates whether to use the start battle skill.
        /// </summary>
        public bool UseStartBattleSkill;

        /// <summary>
        /// The start battle skill.
        /// </summary>
        [ShowIf("UseStartBattleSkill")]
        public EnemySkill startBattleSkill;
        
        /// <summary>
        /// List of enemy skills.
        /// </summary>
        [TableList(AlwaysExpanded = true, DrawScrollView = false)]
        public List<EnemySkill> enemySkills;

        

        private EnemyBase _enemyBase;

        /// <summary>
        /// Gets the next available skill to play.
        /// </summary>
        /// <returns>The next available enemy skill.</returns>
        public EnemySkill GetNextSkill()
        {
            foreach (var enemySkill in enemySkills)
            {
                if (enemySkill.CanPlay())
                {
                    return enemySkill;
                }
            }

            Debug.LogError($"{_enemyBase.name} cannot get a skill for this turn, enemySkill count: {enemySkills.Count}");
            
            return null;
        }

        /// <summary>
        /// Sets the associated enemy for this ability.
        /// </summary>
        /// <param name="enemyBase">The enemy base to set.</param>
        public void SetEnemy(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
            foreach (var skill in enemySkills)
            {
                skill.SetEnemy(enemyBase);
            }

            startBattleSkill?.SetEnemy(enemyBase);
        }

        /// <summary>
        /// Executes actions when the battle starts.
        /// </summary>
        public void OnBattleStart()
        {
            foreach (var skill in enemySkills)
            {
                skill.OnBattleStart();
            }

            startBattleSkill?.OnBattleStart();
        }

        /// <summary>
        /// Updates the cooldowns of the skills.
        /// </summary>
        public void UpdateSkillsCd()
        {
            foreach (var skill in enemySkills)
            {
                skill.UpdateSkillCd();
            }
        }
    }
}
