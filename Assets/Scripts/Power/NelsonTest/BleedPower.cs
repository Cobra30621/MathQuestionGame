using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;

namespace NueGames.Power
{
    public class BleedPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Bleed;

        protected override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                // 造成傷害
                GameActionExecutor.AddToBottom(
                    new DamageAction(1, new List<CharacterBase>() { Owner },
                        GetActionSource(), true));
            }
        }
    }
}