using System.Collections.Generic;
using NueGames.Characters;
using UnityEngine;

namespace EnemyAbility
{
    public class EnemyAbility
    {
        [SerializeField] private EnemyAbilityData _abilityData;
        [SerializeField] private List<EnemySkill> _enemySkills;
        [SerializeField] private EnemySkill _startBattleSkill;
        
        private EnemyBase _enemyBase;

        public EnemyAbility(EnemyAbilityData abilityData, EnemyBase enemyBase)
        {
            _abilityData = abilityData;
            _enemyBase = enemyBase;

            _enemySkills = new List<EnemySkill>();
            foreach (var skillData in _abilityData.enemySkills)
            {
                var enemySkill = new EnemySkill(skillData, enemyBase);
                _enemySkills.Add(enemySkill);
            }

            if (_abilityData.UseStartBattleSkill && _abilityData.StartBattleSkillData != null)
            {
                _startBattleSkill = new EnemySkill(_abilityData.StartBattleSkillData, enemyBase);
            }
        }

        /// <summary>
        /// Gets the next available skillData to play.
        /// </summary>
        /// <returns>The next available enemy skillData.</returns>
        public EnemySkill GetNextSkill()
        {
            foreach (var enemySkill in _enemySkills)
            {
                if (enemySkill.CanPlay())
                {
                    return enemySkill;
                }
            }

            Debug.LogError($"{_enemyBase.name} cannot get a skillData for this turn, enemySkillData count: {_enemySkills.Count}");
            
            return null;
        }

        public bool UseStartBattleSkill()
        {
            return _abilityData.UseStartBattleSkill && _startBattleSkill != null;
        }
        
        public EnemySkill GetStartBattleSkill()
        {
            return _startBattleSkill;
        }


        /// <summary>
        /// Updates the cooldowns of the skills.
        /// </summary>
        public void UpdateSkillsCd()
        {
            foreach (var skill in _enemySkills)
            {
                skill.UpdateSkillCd();
            }
        }
    }
}