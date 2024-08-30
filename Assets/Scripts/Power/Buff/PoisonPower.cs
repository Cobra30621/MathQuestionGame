using System.Collections.Generic;
using Action.Parameters;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.UI;
using UnityEngine;

namespace NueGames.Power
{


    /// <summary>
    /// 中毒能力
    /// </summary>
    public class PoisonPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Poison;
        
        
        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }
        
        


        protected override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                int poisonAmount = Amount;

                // 造成傷害
                var damageInfo = new DamageInfo(poisonAmount, GetActionSource(), fixDamage: true);
                GameActionExecutor.AddAction(new DamageAction(damageInfo, new List<CharacterBase>() {Owner}));
                
                // 扣血後減層數 1 
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                
                
            }
        }
    }
}