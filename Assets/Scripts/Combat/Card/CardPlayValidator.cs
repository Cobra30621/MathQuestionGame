using Characters;

namespace Combat.Card
{
    public static class CardPlayValidator
    {
        /// <summary>
        /// 是否有足夠資源可以使用此卡。
        /// </summary>
        public static bool EnoughResourceToUseCard(BattleCard card)
        {
            return PlayCardJudgment.EnoughResourceToUseCard(card);
        }

        /// <summary>
        /// 根據卡片需求與目標角色判斷是否可以執行使用。
        /// 若卡片需要指定單一敵人，且目標為空或非敵人則無法使用。
        /// </summary>
        public static bool CanUseCard(BattleCard card, CharacterBase target)
        {
            if (!PlayCardJudgment.CanUseCard(card))
            {
                return false;
            }
            
            if (card.SpecifiedEnemyTarget)
            {
                if (target == null || !target.IsCharacterType(CharacterType.Enemy))
                    return false;
            }
            return true;
        }
    }
}