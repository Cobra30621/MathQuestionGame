using System.Collections.Generic;
using Action.Parameters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 新增卡牌至牌組
    /// </summary>
    public class AddCardToDrawAction : GameActionBase
    {
        private List<CardData> _cards;
        
        public AddCardToDrawAction(List<CardData> cards, ActionSource source)
        {
            _cards = cards;
            ActionSource = source;
        }
        
        protected override void DoMainAction()
        {
            foreach (var card in _cards)
            {
                CollectionManager.AddCardToPile(PileType.Draw, 
                    card);
            }
        }
    }
}