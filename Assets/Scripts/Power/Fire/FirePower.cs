using System.Collections.Generic;
using Action.Parameters;
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
    /// 燃燒能力
    /// </summary>
    public class FirePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Fire;
        
        
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
                int fireAmount = Amount;
                if (CombatManager.Instance.MainAlly.HasPower(PowerName.Kindle))
                {
                   fireAmount = Amount * 2;
                }

                // 造成傷害
                var damageInfo = new DamageInfo(fireAmount, GetActionSource(), fixDamage: true);
                GameActionExecutor.AddToBottom(new DamageAction(damageInfo, new List<CharacterBase>() {Owner}));
                
                // 燒血後減層數 1 
                GameActionExecutor.AddToBottom(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                
                
            }
        }
    }
}