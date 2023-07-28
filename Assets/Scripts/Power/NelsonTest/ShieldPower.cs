using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{


    public class ShieldPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Shield;

        public ShieldPower()
        {

        }

        public override void SubscribeAllEvent()
        {
            CombatManager.Instance.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.Instance.OnTurnStart -= OnTurnStart;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == GetOwnerCharacterType())
            {
                ApplyPowerAction action = new ApplyPowerAction();
                action.SetValue(3, 
                    PowerName.Block, 
                    new List<CharacterBase>() {Owner},
                    GetActionSource()
                );
                
                GameActionExecutor.AddToBottom(action);
            }
        }
    }
}