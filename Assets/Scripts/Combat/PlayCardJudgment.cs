using System.Collections.Generic;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace NueGames.Combat
{
    public static class PlayCardJudgment
    {
        public static GameManager GameManager => GameManager.Instance;
        public static CombatManager CombatManager => CombatManager.Instance;

        public static bool CanUseCard(CardBase cardBase)
        {
            return EnoughResourceToUseCard(cardBase);
        }

        public static bool EnoughResourceToUseCard(CardBase cardBase)
        {
            return EnoughMana(cardBase.ManaCost);
        }
        
        private static bool EnoughMana(int needMana)
        {
            return CombatManager.CurrentMana >= needMana;
        }
    }
}