using Characters;
using GameListener;
using NueGames.Enums;

namespace Power.Buff
{
    /// <summary>
    /// 力量
    /// </summary>
    public class StrengthPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Strength;

        public StrengthPower()
        {
            CanNegativeStack = true;
            DamageCalculateOrder = CalculateOrder.AdditionAndSubtraction;
        }

        public override void Init()
        {
            base.Init();
            // 如果是敵人，更新意圖
            if (Owner.IsCharacterType(CharacterType.Enemy))
            {
                var enemy = (global::Characters.Enemy.Enemy) Owner;
                enemy.UpdateIntentionDisplay();
            }
        }


        public override float AtDamageGive(float damage)
        {
            return damage + Amount;
        }
    }
}