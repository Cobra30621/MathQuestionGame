using System;
using Card;
using Card.Data;
using Card.Display;
using Managers;
using NueGames.Data.Collection;
using NueGames.Managers;
using NueGames.UI.Reward;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.Card
{
    public class RewardChoiceCard : MonoBehaviour
    {
        public UICard uiCard;
        private CardRewardChoicePanel _choicePanel;


        public void BuildReward(CardData cardData, CardRewardChoicePanel choicePanel)
        {
            _choicePanel = choicePanel;
            uiCard.Init(cardData);
            uiCard.OnCardChose += OnChoice;
        }

        private void OnChoice()
        {
            _choicePanel.ChoiceCard(this);
        }
    }
}
