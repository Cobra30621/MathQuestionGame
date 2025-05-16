using System.Collections.Generic;
using Characters.Enemy.Data;
using Sheets;
using Sirenix.Utilities;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyAbility
    {
        [SerializeField] private List<EnemySkill> _enemySkills;
        [SerializeField] private List<EnemySkill> _startBattleSkills;
        

        public EnemyAbility(EnemyData data, Enemy enemy, SheetDataGetter getter)
        {
            _enemySkills = data.enemySkillIDs.
                ConvertAll(id => 
                    new EnemySkill(getter.GetEnemySkillInfo(id), enemy, getter));

            if (!data.StartBattleIntentionID.IsNullOrWhitespace())
            {
                _startBattleSkills = new List<EnemySkill>();
                foreach (var skillID in data.startBattleSkillIDs)
                {
                    _startBattleSkills.Add(new EnemySkill(
                        getter.GetEnemySkillInfo(skillID), enemy, getter
                    ));
                }
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

            // Debug.LogError($"{_enemyBase.name} cannot get a skillData for this turn, enemySkillData count: {_enemySkills.Count}");
            
            return _enemySkills[^1];
        }

        public bool UseStartBattleSkill()
        {
            return _startBattleSkills != null;
        }
        
        public List<EnemySkill> GetStartBattleSkill()
        {
            return _startBattleSkills;
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