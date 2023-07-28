using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyBlockAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyBlock;

        public void SetValue(int applyValue,
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            SetPowerActionValue(applyValue, PowerName.Block, targetList, actionSource);
        }
        
        
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.ApplyPower(PowerName.Block, AdditionValue);
            }
        }
    }
}