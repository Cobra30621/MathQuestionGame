using NueGames.Action;
using NueGames.Enums;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyBlockAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyBlock;

        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.ApplyPower(PowerType.Block, AdditionValue);
                PlayFx(FxType.Block, target.transform);
            }
        }
    }
}