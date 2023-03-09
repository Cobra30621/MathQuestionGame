using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class IncreaseStrengthAction : GiveStatusAction
    {
        public override GameActionType ActionType => GameActionType.IncreaseStrength;
        public override PowerType PowerType => PowerType.Strength;
    }
}