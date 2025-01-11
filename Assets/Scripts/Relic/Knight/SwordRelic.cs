using System;
using System.Collections.Generic;
using Action.Parameters;
using Card;
using Combat;
using NueGames.Card;
using System.Collections.Generic;
using Action;
using Action.Power;
using Characters;
using Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Managers;
using Power;
using UnityEngine;
namespace NueGames.Relic.Knight
{
    public class SwordRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Sword;
        public override void SubscribeAllEvent()
        {
            CombatManager.OnBattleStart += OnBattleStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnBattleStart -= OnBattleStart;
        }
        public override int AtGainTurnStartMana(int rawValue)
        {
            return rawValue - 1;
        }

        protected override void OnBattleStart()
        {
            GameActionExecutor.AddAction(new ApplyPowerAction(
                3, PowerName.Strength, new List<CharacterBase>(){MainAlly}, GetActionSource()));

        }
    }
}