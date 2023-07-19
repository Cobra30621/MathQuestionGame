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
            if (info.CharacterType == GetOwnerCharacterType())
            {
                int fireAmount = Amount;
                if (CombatManager.IsMainAllyHasPower(PowerName.Kindle))
                {
                   fireAmount = Amount * 2;
                }

                DamageAction damageAction = new DamageAction();
                damageAction.SetDamageActionValue(fireAmount, 
                    new List<CharacterBase>(){Owner},
                    GetActionSource(),
                    true
                    );
                GameActionExecutor.Instance.AddToBottom(damageAction);
                
                Owner.CharacterStats.ApplyPower(PowerName, -1); // 燒血後減層數 1 
            }
        }
    }
}