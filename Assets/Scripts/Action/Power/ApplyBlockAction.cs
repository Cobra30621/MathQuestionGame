using NueGames.Action;
using NueGames.Enums;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyBlockAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyBlock;
        public override void DoAction()
        {
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ApplyPower(PowerType.Block, AdditionValue);
            
            PlayFx(FxType.Block, Target.transform);
        }
    }
}