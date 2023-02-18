using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveWeakAction : GiveStatusAction
    {
        public override CardActionType ActionType => CardActionType.GiveWeak;
        public override StatusType StatusType => StatusType.Weak;
    }
}