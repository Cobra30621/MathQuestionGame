using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction.DevelopingCard
{
    /// <summary>
    /// 若對方沒有護甲，則額外造成5點傷害
    /// </summary>
    public class StraightSlash : CardActionBase
    {
        public int baseDamage;
        public int extraDamage;
        
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new DamageMoreIfTargetNotContainPowerAction(
                baseDamage, PowerName.Block, extraDamage, TargetList, GetActionSource()));
        }
    }
}