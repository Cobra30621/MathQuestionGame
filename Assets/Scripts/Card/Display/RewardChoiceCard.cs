using System;
using Card;
using Card.Data;
using Card.Display;
using Managers;
using Reward.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.Card
{
    public class RewardChoiceCard : MonoBehaviour
    {
        public UICard uiCard;
        private CardRewardChoicePanel _choicePanel;

        [SerializeField] private GameObject choiceBackground;


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

        public void SetChoiceBackground(bool value)
        {
            choiceBackground.SetActive(value);
        }
    }
}
