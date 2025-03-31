using System.Collections.Generic;
using Characters;
using Combat;
using Combat.Card;
using Effect;
using Effect.Power;
using Managers;
using UI;
using Power;
using Relic.Data;
using Card.Data;
using UnityEngine;

namespace Relic.Mage
{
    public class WandRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Wand;
        
        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                // Get the card with ID "31"
                if (GameManager.Instance.CardManager.cardInfoGetter.GetCardDataWithId("31", out CardData cardData))
                {
                    // Create the card
                    var handController = CollectionManager.Instance.HandController;
                    var clone = GameManager.Instance.BuildAndGetCard(cardData, handController.drawTransform);
                    
                    // Add the card to hand
                    handController.AddCardToHand(clone);
                    CollectionManager.Instance.HandPile.Add(cardData);
                    
                    // Update UI
                    foreach (var cardObject in handController.hand)
                    {
                        cardObject.UpdateCardDisplay();
                    }
                    
                    UIManager.Instance.CombatCanvas.SetPileTexts();
                    if (IsMaxLevel())
                    {
                        // Add the card to hand
                        handController.AddCardToHand(clone);
                        CollectionManager.Instance.HandPile.Add(cardData);
                    
                        // Update UI
                        foreach (var cardObject in handController.hand)
                        {
                            cardObject.UpdateCardDisplay();
                        }
                    
                        UIManager.Instance.CombatCanvas.SetPileTexts();
                    }
                }
                else
                {
                    Debug.LogError("WandRelic: Failed to find card with ID 200001");
                }
            }
        }
    }
}