using System.Collections.Generic;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;
using CombatManager = NueGames.Managers.CombatManager;

namespace NueGames.Combat
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
                return EnoughMana(cardBase.ManaCost) && 
                       EnoughPower(cardData.NeedPowerType, cardBase.PowerCost, CombatManager.GetMainAllyPowerDict());
            }
            else
            {
                return EnoughMana(cardBase.ManaCost);
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
                if (PowerDict[needPower].Amount >= needCount)
                {
                    enoughPower = true;
                }
            }

            return enoughPower;
        }
    }
}