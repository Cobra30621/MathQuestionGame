using Relic.Data;

namespace Relic.NelsonTest
{
    /// <summary>
    /// 答對一題，獲得一層護盾
    /// </summary>
    public class ManaShieldRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.MathShield;


        
        // protected override void OnAnswerCorrect(TurnInfo info)
        // {
        //     if (info.CharacterType == GetOwnerCharacterType())
        //     {
        //         ApplyPowerAction action = new ApplyPowerAction();
        //         action.SetPowerActionValue(1, 
        //             PowerName.Block, 
        //             new List<CharacterBase>() {Owner},
        //             GetActionSource()
        //         );
        //         
        //         GameActionExecutor.AddAction(action);
        //     }
        // }
        
    }
}