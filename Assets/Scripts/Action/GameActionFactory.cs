using System.Collections.Generic;
using Action;
using Action.Parameters;
using Card;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace GameAction
{
    public static class GameActionFactory
    {

        public static List<GameActionBase> GetGameActions(List<SkillInfo> skillInfos, List<CharacterBase> targets, 
            ActionSource actionSource)
        {
            var gameActions = new List<GameActionBase>();
            foreach (var effectInfo in skillInfos)
            {
                var gameAction = GameActionFactory.GetGameAction(effectInfo, targets, actionSource);
                gameActions.Add(gameAction);
                
            }

            return gameActions;
        }
        
        public static GameActionBase GetGameAction(SkillInfo skillInfo, List<CharacterBase> targets, 
            ActionSource actionSource)
        {
            GameActionBase action = BuildAction(skillInfo);

            // 如果 ActionTargetType 指定是玩家，將 TargetList 改成玩家
            // 有可能卡片指定對象是敵人，但有部分效果對應到玩家
            if (skillInfo.Target == ActionTargetType.Ally)
            {
                targets = new List<CharacterBase>(){CombatManager.Instance.MainAlly};
            }
            
            action.SetBasicValue(targets, actionSource);
            return action;
        }

        private static GameActionBase BuildAction(SkillInfo skillInfo)
        {
            switch (skillInfo.EffectID)
            {
                case GameActionType.Damage:
                    return new DamageAction(skillInfo);
                case GameActionType.MultiDamage:
                    return new MultiDamageAction(skillInfo);
                case GameActionType.ApplyPower:
                    return new ApplyPowerAction(skillInfo);
                case GameActionType.Block:
                    return new ApplyBlockAction(skillInfo);
                case GameActionType.MultiBlock:
                    return new MultiBlockAction(skillInfo);
                case GameActionType.GainMana:
                    return new GainManaAction(skillInfo);
                default:
                    Debug.LogError("无效的技能类型");
                    return new NullAction(skillInfo);
            }
        }
    }
}