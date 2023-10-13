using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Power
{
    public class SelfDestructPower : PowerBase
    {
        public override PowerName PowerName => PowerName.SelfDestruct;
        int _turnCount = 0;
        public SelfDestructPower()
        {
            
        }
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
            _turnCount++;
            if (_turnCount >= 3)
            {   //Owner改成玩家ｓ
                GameActionExecutor.AddToBottom(new DamageAction(
                    20,new List<CharacterBase>() {Owner},
                    GetActionSource(), fixDamage:true));
            }    
        }
    }
}