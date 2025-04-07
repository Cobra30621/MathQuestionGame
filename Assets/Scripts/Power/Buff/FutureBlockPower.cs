using Characters;
using Combat;
using System.Collections.Generic;
using Effect;
using Effect.Power;
namespace Power.Buff
{
    /// <summary>
    /// 下一回合，額外獲得層數點護盾
    /// </summary>
    public class FutureBlockPower : PowerBase
    {
        public override PowerName PowerName => PowerName.FutureBlock;

        public override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == CharacterType.Ally)
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    Amount, PowerName.Block, new List<CharacterBase>() {Owner},
                    GetEffectSource()));
                ClearPower();
            }
        }
    }
}