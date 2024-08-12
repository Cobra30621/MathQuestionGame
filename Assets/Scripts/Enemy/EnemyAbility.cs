using System.Collections.Generic;
using Sheets;
using Tool;
using UnityEngine;

namespace Enemy.Data
{
    public class EnemyAbility
    {
        [SerializeField] private List<EnemySkill> _enemySkills;
        [SerializeField] private EnemySkill _startBattleSkill;
        

        public EnemyAbility(EnemyData data, EnemyBase enemyBase, SheetDataGetter getter)
        {
            _enemySkills = data.enemySkillIDs.
                ConvertAll(id => 
                    new EnemySkill(getter.GetEnemySkillInfo(id), enemyBase, getter));

            _startBattleSkill = new EnemySkill(
                getter.GetEnemySkillInfo(data.StartBattleSkillID), enemyBase, getter);
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
            return _startBattleSkill != null;
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