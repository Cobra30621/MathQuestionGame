using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveStunAction : GiveStatusAction
    {
        public override CardActionType ActionType => CardActionType.GiveStun;
        public override StatusType StatusType => StatusType.Stun;
    }
}