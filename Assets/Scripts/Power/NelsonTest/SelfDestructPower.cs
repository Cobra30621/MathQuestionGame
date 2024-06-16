using System.Collections.Generic;
using Action.Parameters;
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
            {   
                var damageInfo = new DamageInfo(20, GetActionSource(), fixDamage: true);

                GameActionExecutor.AddToBottom(new DamageAction(damageInfo, new List<CharacterBase>() {Owner}));
                
            }    
        }
    }
}