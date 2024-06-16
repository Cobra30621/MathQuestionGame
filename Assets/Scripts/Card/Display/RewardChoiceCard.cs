using System;
using Card.Data;
using Card.Display;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.Card
{
    public class RewardChoiceCard : MonoBehaviour
    {
        public GameManager GameManager => GameManager.Instance;
        public UIManager UIManager => UIManager.Instance;

        public UICard uiCard;
        
        
        public void BuildReward(CardData cardData)
        {
            uiCard.Init(cardData);
            uiCard.OnCardChose += OnChoice;
        }


        private void OnChoice()
        {
            if (GameManager != null)
                GameManager.CurrentCardsList.Add(uiCard.CardData);

            if (UIManager != null)
                UIManager.RewardCanvas.ChoicePanel.DisablePanel();
        }


        
    }
}
