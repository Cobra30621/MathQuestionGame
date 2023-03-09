using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveVulnerableAction : GiveStatusAction
    {
        public override GameActionType ActionType => GameActionType.GiveVulnerable;
        public override PowerType PowerType => PowerType.Vulnerable;
    }
}