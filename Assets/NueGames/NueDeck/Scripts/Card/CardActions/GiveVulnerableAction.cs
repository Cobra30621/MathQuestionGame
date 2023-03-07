using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveVulnerableAction : GiveStatusAction
    {
        public override CardActionType ActionType => CardActionType.GiveVulnerable;
        public override PowerType PowerType => PowerType.Vulnerable;
    }
}