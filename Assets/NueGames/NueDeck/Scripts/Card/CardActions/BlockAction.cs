using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class BlockAction : GiveStatusAction
    {
        public override CardActionType ActionType => CardActionType.Block;
        public override PowerType PowerType => PowerType.Block;
        public override FxType FxType => FxType.Block;
    }
}