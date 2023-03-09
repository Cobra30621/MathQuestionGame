using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveWeakAction : GiveStatusAction
    {
        public override GameActionType ActionType => GameActionType.GiveWeak;
        public override PowerType PowerType => PowerType.Weak;
    }
}