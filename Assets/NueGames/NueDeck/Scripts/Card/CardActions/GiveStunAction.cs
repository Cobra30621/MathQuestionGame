using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class GiveStunAction : GiveStatusAction
    {
        public override GameActionType ActionType => GameActionType.GiveStun;
        public override PowerType PowerType => PowerType.Dexterity;
    }
}