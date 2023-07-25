using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction.Common
{
    public class ApplyPowerCard : CardActionBase
    {
        public int Amount;
        public PowerName PowerName;
        public int AddAmonut;
        public override void DoAction()
        {
            var applyPowerAction = new ApplyPowerAction();
            
            applyPowerAction.SetPowerActionValue(Amount, PowerName, TargetList, GetActionSource());
            applyPowerAction.SetFXValue(FxName.Buff, FxSpawnPosition.EachTarget);
            
            GameActionExecutor.Instance.AddToBottom(applyPowerAction);
        }
    }
}