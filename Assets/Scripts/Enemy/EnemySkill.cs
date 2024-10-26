using System;
using System.Collections.Generic;
using Action.Parameters;
using Card;
using Combat;
using GameAction;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;
using NueGames.Parameters;
using Sheets;
using Tool;
using UnityEngine;

namespace Enemy.Data
{
    /// <summary>
    /// 敌人技能类,用于管理敌人的技能相关信息和行为
    /// </summary>
    [Serializable]
    public class EnemySkill
    {
         #region Variable
         [SerializeField] private int currentCd; // 当前冷却时间
         
         private EnemyBase _enemyBase; // 敌人基类引用

         private EnemySkillData _skillData; // 技能数据

         public List<SkillInfo> skillInfos; // 技能信息列表

         public Intention _intention; // 意图
         
         // 内部系统用
         public bool useSheetInfos; // 是否使用表格信息
         public List<GameActionBase> _gameActions; // 游戏动作列表
         
         #endregion

         /// <summary>
         /// 使用表格数据初始化敌人技能
         /// </summary>
         public EnemySkill(EnemySkillData skillData, EnemyBase enemyBaseBase, SheetDataGetter getter)
         {
             _skillData = skillData;
             _enemyBase = enemyBaseBase;
             
             currentCd = 0;
             skillInfos = _skillData.skillIDs.ConvertAll(getter.GetSkillInfo);
             _intention = getter.GetIntention(skillData.Intention);

             useSheetInfos = true;
         }

         /// <summary>
         /// 设定在内部系统行动
         /// </summary>
         public EnemySkill(List<GameActionBase> gameActions, Intention intention)
         {
             _gameActions = gameActions;
             _intention = intention;
             useSheetInfos = false;
         }

        /// <summary>
        /// 执行技能
        /// </summary>
        public void PlaySkill()
        {
            // 更新 CD
            if(useSheetInfos)
                currentCd = _skillData.CD;
            
            ActionSource actionSource = new ActionSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = _enemyBase
            };

            if (useSheetInfos)
            {
                _gameActions = GameActionFactory.GetGameActions(skillInfos,
                    new List<CharacterBase>() { _enemyBase }, actionSource);
            }
            
            GameActionExecutor.AddAction(_gameActions, 0.5f);
        }

        /// <summary>
        /// 更新技能冷却时间
        /// </summary>
        public void UpdateSkillCd()
        {
            currentCd--;
            if (currentCd < 0)
            {
                currentCd = 0;
            }
        }

        /// <summary>
        /// 获取意图值
        /// </summary>
        /// <param name="value">输出的意图值</param>
        /// <returns>是否成功获取意图值</returns>
        public bool GetIntentionValue(out int value)
        {
            // Debug.Log($"Enemy Skill {_skillData}, Enemy {_enemy.name}");
            if (_intention.ShowIntentionValue)
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
        /// 检查技能是否可以使用
        /// </summary>
        /// <returns>技能是否可以使用</returns>
        public bool CanPlay()
        {
            return currentCd <= 0 ;
        }
    }
}
