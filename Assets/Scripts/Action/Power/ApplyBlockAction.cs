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
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.ApplyPower(PowerType.Block, AdditionValue);
            
            PlayFx();
            PlayAudio();
        }
    }
}