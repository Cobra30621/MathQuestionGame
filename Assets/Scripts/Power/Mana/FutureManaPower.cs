using Characters;
using Combat;

namespace Power.Mana
{
    /// <summary>
    /// 下一回合，額外獲得層數點魔力
    /// </summary>
    public class FutureManaPower : PowerBase
    {
        public override PowerName PowerName => PowerName.FutureMana;
        
        public override int AtGainTurnStartMana(int rawValue)
        {
            
            return rawValue + Amount;
        }

        public override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == CharacterType.Ally)
            {
                ClearPower();
            }
        }
    }
}