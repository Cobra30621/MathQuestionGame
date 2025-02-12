﻿using Managers;

namespace Combat.Card
{
    public static class PlayCardJudgment
    {
        public static GameManager GameManager => GameManager.Instance;
        public static CombatManager CombatManager => CombatManager.Instance;

        public static bool CanUseCard(BattleCard battleCard)
        {
            return EnoughResourceToUseCard(battleCard) && !battleCard.CardData.CanNotPlay;
        }

        public static bool EnoughResourceToUseCard(BattleCard battleCard)
        {
            return EnoughMana(battleCard.ManaCost);
        }
        
        private static bool EnoughMana(int needMana)
        {
            return CombatManager.CurrentMana >= needMana;
        }
    }
}