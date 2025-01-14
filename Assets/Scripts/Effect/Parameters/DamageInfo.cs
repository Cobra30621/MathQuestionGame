using Characters;
using Combat;
using Power;

namespace Effect.Parameters
{
    /// <summary>
    /// 傷害設定參數
    /// </summary>
    public class DamageInfo
    {
        /// <summary>
        /// 傷害來源
        /// </summary>
        public EffectSource EffectSource { get; private set; }
        /// <summary>
        /// 傷害對象
        /// </summary>
        public CharacterBase Target { get; private set; }
        /// <summary>
        /// 基礎傷害數值
        /// </summary>
        public float DamageValue { get; private set; }
        /// <summary>
        /// 固定傷害，不受狀態影響
        /// </summary>
        public bool FixDamage { get; private set; }
        /// <summary>
        /// 可以穿甲
        /// </summary>
        public bool CanPierceArmor { get; private set; }

        
        public DamageInfo(float damageValue, EffectSource effectSource, bool fixDamage = false, bool canPierceArmor = false)
        {
            EffectSource = effectSource;
            DamageValue = damageValue;
            FixDamage = fixDamage;
            CanPierceArmor = canPierceArmor;
        }

        public void SetTarget(CharacterBase target)
        {
            Target = target;
        }


        public int GetDamageValue()
        {
            return CombatCalculator.GetDamageValue(this);
        }


        /// <summary>
        /// 傷害是否可以被格檔
        /// </summary>
        /// <returns></returns>
        public bool EnableBlock()
        {
            var target = Target;
            var damageValue = GetDamageValue();
            
            if (CanPierceArmor)
            {
                return false;
            }
            
            if (target.HasPower(PowerName.Block))
            {
                var blockValue = target.GetPowerValue(PowerName.Block);

                return blockValue >= damageValue;
            }

            return false;
        }


        /// <summary>
        /// 本次傷害有讓對方受到傷害
        /// </summary>
        /// <returns></returns>
        public bool HaveDamage()
        {
            int afterBlockDamage = GetAfterBlockDamage();

            return afterBlockDamage != 0;
        }
        
        
        /// <summary>
        /// 取得格檔後剩下的傷害
        /// </summary>
        public int GetAfterBlockDamage()
        {
            var target = Target;
            var damageValue = GetDamageValue();

            if (target == null)
            {
                return damageValue;
            }
            
            var remainingDamage = damageValue;
            if (!CanPierceArmor)
            {
                if (target.HasPower(PowerName.Block))
                {
                    var blockValue = target.GetPowerValue(PowerName.Block);
                    remainingDamage -= blockValue;

                    if (remainingDamage < 0)
                    {
                        remainingDamage = 0;
                    }
                }
            }

            return remainingDamage;
        }

        public override string ToString()
        {
            return
                $"來源: {EffectSource}\n" +
                $"目標: {Target}\n" +
                $"傷害: {GetDamageValue()}" +
                $"格檔後傷害: {GetAfterBlockDamage()}";
        }
    }
}