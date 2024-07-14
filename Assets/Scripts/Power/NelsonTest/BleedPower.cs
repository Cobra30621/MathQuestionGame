using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;

namespace NueGames.Power
{
    public class BleedPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Bleed;
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
                var damageInfo = new DamageInfo(1, GetActionSource(), fixDamage: true, canPierceArmor:true);

                GameActionExecutor.AddAction(new DamageAction(damageInfo, new List<CharacterBase>() {Owner}));
            }
        }
    }
}