using NueGames.Action;
using NueGames.Card;
using NueGames.Enums;

namespace Action.MathMana
{
    /// <summary>
    /// 將所有帶有"法力(MathMana)"關鍵字的卡片，耗能變成 0
    /// </summary>
    public class UnlimitedUseMathManaCardAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.UnlimitedUseMathManaCard;
        
        public override void DoAction()
        {
            CollectionManager.ChangeHandCardManaCost(SpecialKeywords.MathMana, 0, true);
            CollectionManager.ChangeHandCardManaCost(SpecialKeywords.MathMana, 0, false);
        }
    }
}