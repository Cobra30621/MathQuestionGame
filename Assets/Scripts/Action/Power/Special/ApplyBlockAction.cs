using NueGames.Action;
using NueGames.Enums;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyBlockAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyBlock;

        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.ApplyPower(PowerName.Block, AdditionValue);
            }
        }
    }
}