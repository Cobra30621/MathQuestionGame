using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace EnemyAbility.EnemyAction
{
    public class ApplyPower : EnemyActionBase
    {
        public override int DamageValueForIntention => -1;
        public override bool IsDamageAction => false;

        
        public PowerName PowerName;
        public int PowerAmount;
        protected override void DoMainAction()
        {
            Debug.Log($"Do {PowerName} {PowerAmount}");
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                PowerAmount, PowerName, TargetList, GetActionSource()));
        }
    }
}