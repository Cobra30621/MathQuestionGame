using System.Collections.Generic;
using Characters;
using Combat;
using Effect.Card;
using Effect.Common;
using Effect.Condition;
using Effect.Damage;
using Effect.Enemy;
using Effect.Other;
using Effect.Parameters;
using Effect.Power;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// 效果工廠，用於根據技能資訊產生具體的遊戲效果。
    /// 通常由卡牌、敵人、道具等系統透過表單資料使用。
    /// </summary>
    public static class EffectFactory
    {
        /// <summary>
        /// 根據多筆技能資訊與指定目標，建立所有對應效果。
        /// </summary>
        /// <param name="skillInfos">技能資訊清單</param>
        /// <param name="targets">目標角色列表</param>
        /// <param name="effectSource">效果來源（如來源角色或卡牌）</param>
        public static List<EffectBase> GetEffects(List<SkillInfo> skillInfos, List<CharacterBase> targets, EffectSource effectSource)
        {
            var effects = new List<EffectBase>();

            foreach (var skillInfo in skillInfos)
            {
                var effect = GetEffect(skillInfo, targets, effectSource);
                effects.Add(effect);
            }

            return effects;
        }

        /// <summary>
        /// 根據單一技能資訊建立對應效果，並套用目標與來源。
        /// </summary>
        /// <param name="skillInfo">技能資訊</param>
        /// <param name="specifiedTargets">指定目標角色列表</param>
        /// <param name="effectSource">效果來源</param>
        public static EffectBase GetEffect(SkillInfo skillInfo, List<CharacterBase> specifiedTargets, EffectSource effectSource)
        {
            EffectBase effect = BuildEffect(skillInfo);

            var targets = ResolveTargets(skillInfo.Target, specifiedTargets);
            effect.SetBasicValue(targets, effectSource);

            return effect;
        }

        /// <summary>
        /// 根據目標類型（TargetType）解析出具體的角色列表。
        /// </summary>
        /// <param name="targetType">目標類型</param>
        /// <param name="specifiedTargets">外部指定的角色列表（可用於指定敵人）</param>
        public static List<CharacterBase> ResolveTargets(ActionTargetType targetType, List<CharacterBase> specifiedTargets)
        {
            List<CharacterBase> targetList = new();

            switch (targetType)
            {
                case ActionTargetType.Ally:
                    targetList.Add(CombatManager.Instance.MainAlly);
                    break;
                case ActionTargetType.SpecifiedEnemy:
                    targetList = specifiedTargets;
                    break;
                case ActionTargetType.AllEnemies:
                    targetList.AddRange(CombatManager.Instance.Enemies);
                    break;
                case ActionTargetType.RandomEnemy:
                    targetList.Add(CombatManager.Instance.RandomEnemy);
                    break;
                default:
                    Debug.LogWarning($"未處理的目標類型：{targetType}");
                    break;
            }

            return targetList;
        }

        /// <summary>
        /// 根據 EffectName 創建對應的 Effect 實例。
        /// 每個 Effect 對應一個類別。
        /// </summary>
        /// <param name="skillInfo">技能資訊</param>
        /// <returns>具體效果類別</returns>
        private static EffectBase BuildEffect(SkillInfo skillInfo)
        {
            return skillInfo.EffectID switch
            {
                EffectName.Damage => new DamageEffect(skillInfo),
                EffectName.ApplyBlock => new ApplyBlockEffect(skillInfo),
                EffectName.ApplyPower => new ApplyPowerEffect(skillInfo),
                EffectName.Heal => new HealEffect(skillInfo),
                EffectName.AddCardToPile => new AddCardToPileEffect(skillInfo),
                EffectName.EnemyHpDamage => new EnemyHpDamageEffect(skillInfo),
                EffectName.GainMana => new GainManaEffect(skillInfo),
                EffectName.BlockByCount => new BlockByCountEffect(skillInfo),
                EffectName.DamageByCount => new DamageByCountEffect(skillInfo),
                EffectName.DrawCard => new DrawCardEffect(skillInfo),
                EffectName.ClearPower => new ClearPowerEffect(skillInfo),
                EffectName.DrawMoreCardWhenCondition => new DrawMoreCardWhenConditionEffect(skillInfo),
                EffectName.AddMoreBlockWhenCondition => new AddMoreBlockWhenConditionEffect(skillInfo),
                EffectName.AddManaWhenCondition => new AddManaWhenConditionEffect(skillInfo),
                EffectName.DamageMoreWhenUseEnoughCard => new DamageMoreWhenUseEnoughCardEffect(skillInfo),
                EffectName.EndPlayerTurn => new EndPlayerTurnEffect(skillInfo),
                EffectName.DoEffectWhenCondition => new DoEffectWhenCondition(skillInfo),
                EffectName.SpawnEnemy => new SpawnEnemyEffect(skillInfo),
                EffectName.SplitEnemy => new SplitEnemyEffect(skillInfo),
                EffectName.DemonicSacrifice => new DemonicSacrificeEffect(skillInfo),
                _ => HandleInvalidEffect(skillInfo)
            };
        }

        /// <summary>
        /// 處理未實作的 EffectName，回傳 NullEffect 並記錄錯誤。
        /// </summary>
        /// <param name="skillInfo">錯誤來源技能</param>
        private static EffectBase HandleInvalidEffect(SkillInfo skillInfo)
        {
            Debug.LogError($"無效的 EffectName: {skillInfo.EffectID} 來自技能 ID: {skillInfo.SkillID}");
            return new NullEffect(skillInfo);
        }
    }
}
