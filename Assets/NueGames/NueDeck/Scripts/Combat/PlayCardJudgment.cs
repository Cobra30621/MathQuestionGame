using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using CombatManager = NueGames.NueDeck.Scripts.Managers.CombatManager;

namespace NueGames.NueDeck.Scripts.Combat
{
    public static class PlayCardJudgment
    {
        public static GameManager GameManager => GameManager.Instance;

        public static bool CanUseCard(CardBase cardBase)
        {
            return GameManager.PersistentGameplayData.CanUseCards && EnoughManaToUseCard(cardBase);
        }

        public static bool EnoughManaToUseCard(CardBase cardBase)
        {
            CardData cardData = cardBase.CardData;
            if (cardData.NeedMathManaToPlay)
            {
                return EnoughMana(cardData.ManaCost) && EnoughMathMana(cardData.MathManaCost);
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
        
        private static bool EnoughMathMana(int needMathMana)
        {
            return GameManager.PersistentGameplayData.CurrentMathMana >= needMathMana;
        }
    }
}