using System;
using System.Collections.Generic;
using Action.Parameters;
using Card;
using GameAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;
using NueGames.Parameters;
using UnityEngine;

namespace Enemy.Data
{
    [Serializable]
    public class EnemySkill
    {
         #region Variable
         [SerializeField] private int currentCd;
         
         private EnemyBase _enemyBase;

         private EnemySkillData _skillData;

         public List<SkillInfo> skillInfos;

         public Intention intention;
         
         #endregion

         public EnemySkill(EnemySkillData skillData, EnemyBase enemyBaseBase, SheetDataGetter getter)
         {
             _skillData = skillData;
             _enemyBase = enemyBaseBase;
             
             currentCd = 0;
             skillInfos = _skillData.skillIDs.ConvertAll(getter.GetSkillInfo);
             intention = getter.GetIntention(skillData.Intention);
         }



        /// <summary>
        /// Plays the skillData on the provided target list.
        /// </summary>
        /// <param name="targetList">The list of target characters.</param>
        public void PlaySkill()
        {
            currentCd = _skillData.CD;
            
            ActionSource actionSource = new ActionSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = _enemyBase
            };

            var gameActions = GameActionFactory.GetGameActions(skillInfos,
                new List<CharacterBase>() { _enemyBase }, actionSource);
            foreach (var gameAction in gameActions)
            {
                GameActionExecutor.AddAction(gameAction, 0);
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
            // Debug.Log($"Enemy Skill {_skillData}, Enemy {_enemy.name}");
            if (intention.ShowIntentionValue)
            {
                value = -1;
                
                // TODO : 
                var damageInfo = new DamageInfo(-1,
                    new ActionSource()
                    {
                        SourceType = SourceType.Enemy,
                        SourceCharacter = _enemyBase
                    }
                    );
                damageInfo.SetTarget(CombatManager.Instance.MainAlly);
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
            return currentCd <= 0 ;
        }
    }
}