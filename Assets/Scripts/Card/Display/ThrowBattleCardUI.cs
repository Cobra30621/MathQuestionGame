using System;
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
            GameManager.Instance.ThrowCard( uiCard.CardData);
        }

    }
}