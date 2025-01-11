using System;

namespace Power
{
    public class PowerHelper
    {
        public static PowerName GetPowerName(int powerIndex)
        {
            var powerName = (PowerName)powerIndex;

            if (powerName == PowerName.None)
            {
                throw new Exception($"PowerName 沒有編號 {powerIndex}");
            }

            return powerName;
        }
    }
}