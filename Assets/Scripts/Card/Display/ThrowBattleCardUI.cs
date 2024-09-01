using System;
using Card;
using Card.Data;
using Card.Display;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Card
{
    public class ThrowBattleCardUI : MonoBehaviour
    {
        public UICard uiCard;

        
        public void Init(CardData cardData)
        {
            uiCard.Init(cardData);
            uiCard.OnCardChose += ThrowCard;
        }
        
        
        public void ThrowCard()
        {
            CardManager.Instance.ThrowCard( uiCard.CardData);
        }

    }
}