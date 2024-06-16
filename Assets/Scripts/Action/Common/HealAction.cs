using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 回血
    /// </summary>
    public class HealAction : GameActionBase
    {
        /// <summary>
        /// 回復血量
        /// </summary>
        private int _healValue;
        
        public HealAction(int healValue, List<CharacterBase> targetList, ActionSource actionSource)
        {
            _healValue = healValue;
            TargetList = targetList;
            ActionSource = actionSource;
        }

        /// <summary>
        /// 讀表用
        /// </summary>
        /// <param name="skillInfo"></param>
        public HealAction(SkillInfo skillInfo)
        {
            _healValue = skillInfo.int1;
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.Heal(_healValue);

                PlaySpawnTextFx($"{_healValue}", target.TextSpawnRoot);
            }
            
        }
    }
}