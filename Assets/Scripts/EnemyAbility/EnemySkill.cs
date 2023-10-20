using System.Collections.Generic;
using Action.Parameters;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Parameters;
using UnityEngine;

namespace EnemyAbility
{
    [SelectionBase]
    public class EnemySkill
    {
        #region Skill Data

        private EnemySkillData _skillData;

        public bool UseDefaultAttackFeedback => _skillData.UseDefaultAttackFeedback;
        public bool UseCustomFeedback => _skillData.UseCustomFeedback;
        public string CustomFeedbackKey => _skillData.CustomFeedbackKey;
        public GameObject FxGo => _skillData.FxGo;
        public FxSpawnPosition FxSpawnPosition => _skillData.FxSpawnPosition;
        public EnemyIntentionData Intention => _skillData.Intention;
        

        #endregion
        
         #region Variable

         [SerializeField] private ConditionBase _condition;
         [SerializeField] private int currentCd;
         [SerializeField] private int hasUsedCount;
         private EnemyBase _enemy;

         #endregion

         public EnemySkill(EnemySkillData skillData, EnemyBase enemyBase)
         {
             _skillData = skillData;
             _enemy = enemyBase;
             
             _condition = _skillData.GetCopyCondition();
             _condition?.SetEnemy(enemyBase);
             
             currentCd = 0;
             hasUsedCount = 0;
         }



        /// <summary>
        /// Plays the skillData on the provided target list.
        /// </summary>
        /// <param name="targetList">The list of target characters.</param>
        public void PlaySkill()
        {
            currentCd = _skillData.SkillCd;
            hasUsedCount++;

            if (_skillData.EnemyActions == null)
            {
                Debug.LogError($"Enemy {_enemy.name} 的技能 {_skillData.Name} 的 EnemyActions 為空的" +
                               $"請去設定");
                return;
            }
            
            Debug.Log($"_skillData.EnemyActions {_skillData.EnemyActions.Count}");
            foreach (var enemyAction in _skillData.EnemyActions)
            {
                Debug.Log($"enemyAction {enemyAction} {enemyAction.ActionTargetType}");
                var targetList = CombatManager.Instance.EnemyDetermineTargets(
                    _enemy, enemyAction.ActionTargetType);
                enemyAction.SetValue(_enemy, this, targetList);
                enemyAction.DoAction();
            }
        }

        /// <summary>
        /// Updates the cooldown of the skillData.
        /// </summary>
        public void UpdateSkillCd()
        {
            currentCd--;
            if (currentCd < 0)
            {
                currentCd = 0;
            }
        }


        public bool GetIntentionValue(out int value)
        {
            Debug.Log($"Enemy Skill {_skillData}, Enemy {_enemy.name}");
            if (_skillData.Intention.ShowIntentionValue)
            {
                value = -1;
                if (_skillData.EnemyActions.Count == 0)
                {
                    return false;
                }
                if (!_skillData.EnemyActions[0].IsDamageAction) return false;
                
                var damageInfo = new DamageInfo()
                {
                    Target = CombatManager.Instance.MainAlly,
                    damageValue = _skillData.EnemyActions[0].DamageValueForIntention,
                    ActionSource = new ActionSource()
                    {
                        SourceType = SourceType.Enemy,
                        SourceCharacter = _enemy
                    }
                };
                value = CombatCalculator.GetDamageValue(damageInfo);
                return true;
            }

            value = -1;
            return false;
        }


        /// <summary>
        /// Checks if the skillData can be played.
        /// </summary>
        /// <returns>True if the skillData can be played, otherwise false.</returns>
        public bool CanPlay()
        {
            return currentCd <= 0 && CheckCondition() && CheckUseCount();
        }

        /// <summary>
        /// Checks the condition for using the skillData.
        /// </summary>
        /// <returns>True if the condition is met, otherwise false.</returns>
        private bool CheckCondition()
        {
            return !_skillData.UseCondition || (_condition?.Judge() ?? true);
        }

        /// <summary>
        /// Checks the use count of the skillData.
        /// </summary>
        /// <returns>True if the use count condition is met, otherwise false.</returns>
        private bool CheckUseCount()
        {
            return (hasUsedCount <_skillData.MaxUseCount) || (_skillData.MaxUseCount == -1);
        }
    }
}