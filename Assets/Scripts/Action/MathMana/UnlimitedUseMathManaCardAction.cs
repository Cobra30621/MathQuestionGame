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
        public override ActionName ActionName => ActionName.UnlimitedUseMathManaCard;

        protected override void DoMainAction()
        {
            CollectionManager.ChangeHandCardManaCost(SpecialKeywords.MathMana, 0);
        }
    }
}