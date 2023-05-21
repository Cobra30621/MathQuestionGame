using NueGames.Characters;
using NueGames.Enums;

namespace NueGames.Parameters
{
    public class ApplyPowerParameters
    {
        /// <summary>
        /// 傷害對象
        /// </summary>
        public CharacterBase Target;
        /// <summary>
        /// 能力名稱
        /// </summary>
        public PowerType PowerType;
        /// <summary>
        /// 傷害數值
        /// </summary>
        public int Value;


        public ApplyPowerParameters(CharacterBase target, PowerType powerType, int value)
        {
            Target = target;
            PowerType = powerType;
            Value = value;
            
            
        }

        public ApplyPowerParameters(int value, PowerType powerType)
        {
            PowerType = powerType;
            Value = value;
        }
    }
}