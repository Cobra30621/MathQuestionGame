using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Power;
using UnityEngine;
using CombatManager = NueGames.NueDeck.Scripts.Managers.CombatManager;

namespace NueGames.NueDeck.Scripts.Combat
{
    public static class PlayCardJudgment
    {
        public static GameManager GameManager => GameManager.Instance;
        public static CombatManager CombatManager => CombatManager.Instance;

        public static bool CanUseCard(CardBase cardBase)
        {
            return GameManager.PersistentGameplayData.CanUseCards && EnoughResourceToUseCard(cardBase);
        }

        public static bool EnoughResourceToUseCard(CardBase cardBase)
        {
            CardData cardData = cardBase.CardData;
            if (cardData.NeedPowerToPlay)
            {
                return EnoughMana(cardData.ManaCost) && 
                       EnoughPower(cardData.NeedPowerType, cardData.PowerCost, CombatManager.GetMainAllyPowerDict());
            }
            else
            {
                return EnoughMana(cardData.ManaCost);
            }
        }
        
        private static bool EnoughMana(int needMana)
        {
            return GameManager.PersistentGameplayData.CurrentMana >= needMana;
        }
        

        private static bool EnoughPower(PowerType needPower, int needCount, Dictionary<PowerType, PowerBase> PowerDict)
        {
            bool enoughPower = false;
            if (PowerDict.ContainsKey(needPower))
            {
                if (PowerDict[needPower].Value >= needCount)
                {
                    enoughPower = true;
                }
            }

            return enoughPower;
        }
    }
}