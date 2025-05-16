using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Enemy.Data;
using Combat;
using Effect;
using Effect.Parameters;
using Log;
using Sheets;
using Sirenix.Utilities;
using UnityEngine;

namespace Characters.Enemy
{
    /// <summary>
    /// 敌人技能类,用于管理敌人的技能相关信息和行为
    /// </summary>
    [Serializable]
    public class EnemySkill
    {
         #region Variable
         [SerializeField] private int currentCd; // 当前冷却时间
         
         private Enemy _enemy; // 敌人基类引用

         private EnemyIntentionInfo _intentionInfo; // 技能数据

         public List<SkillInfo> skillInfos; // 技能信息列表

         public Intention _intention; // 意图
         
         // 内部系统用
         public bool useSheetInfos; // 是否使用表格信息
         public List<EffectBase> _gameActions; // 游戏动作列表

         private string _skillName;
         
         #endregion

         /// <summary>
         /// 使用表格数据初始化敌人技能
         /// </summary>
         public EnemySkill(EnemyIntentionInfo intentionInfo, Enemy enemy, SheetDataGetter getter)
         {
             _intentionInfo = intentionInfo;
             _enemy = enemy;
             
             currentCd = 0;
             skillInfos = _intentionInfo.skillIDs.ConvertAll(getter.GetSkillInfo);
             _intention = getter.GetIntention(intentionInfo.Intention);
             _skillName = intentionInfo.ps;

             useSheetInfos = true;
         }

         /// <summary>
         /// 设定在内部系统行动
         /// </summary>
         public EnemySkill(List<EffectBase> gameActions, Intention intention, 
             Enemy enemy, string skillName)
         {
             _gameActions = gameActions;
             _intention = intention;
             _enemy = enemy;
             _skillName = skillName;
             useSheetInfos = false;
         }

        /// <summary>
        /// 执行技能
        /// </summary>
        public void PlaySkill()
        {
            // 更新 CD
            if(useSheetInfos)
                currentCd = _intentionInfo.CD;
            
            var actionSource = GetEffectSource();

            if (useSheetInfos)
            {
                _gameActions = EffectFactory.GetEffects(skillInfos,
                    new List<CharacterBase>() { _enemy }, actionSource);
            }
            
            EffectExecutor.AddEffect(_gameActions, 0.5f);
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
        /// <param name="info">输出的意图值</param>
        /// <returns>是否成功获取意图值</returns>
        public bool GetIntentionValue(out string info)
        {
            if (_intention.ShowIntentionValue)
            {
                _gameActions = EffectFactory.GetEffects(skillInfos,
                    new List<CharacterBase>() { _enemy }, GetEffectSource());
                
                var (damage, times) = _gameActions[0].GetDamageBasicInfo();

                if (damage == -1)
                {
                    Debug.LogError($"{_gameActions[0]} 請設置 GetDamage Basic Info Function，以此顯示傷害數值");
                }
                
                var damageInfo = new DamageInfo(damage,
                    new EffectSource()
                    {
                        SourceType = SourceType.Enemy,
                        SourceCharacter = _enemy
                    }
                    );
                damageInfo.SetTarget(CombatManager.Instance.MainAlly);
                var finalDamage = CombatCalculator.GetDamageValue(damageInfo);

                info = times > 1 ? $"{finalDamage} x {times}" : $"{finalDamage}";
                
                return true;
            }

            info = "";
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

        public string SkillName()
        {
            return _skillName;
        }
        
        private EffectSource GetEffectSource()
        {
            EffectSource effectSource = new EffectSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = _enemy
            };
            return effectSource;
        }

        public override string ToString()
        {
            string skillInfoString = "";
            if(skillInfos != null)
                skillInfoString = string.Join(", ", skillInfos.Select(s => s.ToString()));
            return
                $"技能: {skillInfoString}\n" +
                $"currentCD: {currentCd}, {GetEffectSource()}";
        }
    }
}
