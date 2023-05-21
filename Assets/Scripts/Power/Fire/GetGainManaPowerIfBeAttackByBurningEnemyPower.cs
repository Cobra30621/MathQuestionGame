using Cinemachine;
using NueGames.Enums;
using NueGames.Parameters;

namespace NueGames.Power
{
    /// <summary>
    /// 火舞: 遭受到來自燃燒狀態的敵人攻擊時 下一回合魔力額外增加1 
    /// </summary>
    public class GetGainManaPowerIfBeAttackByBurningEnemyPower : PowerBase
    {
        public override PowerType PowerType => PowerType.GetGainManaPowerIfBeAttackByBurningEnemy;


        public override void SubscribeAllEvent()
        {
            Owner.CharacterStats.OnAttacked += OnAttacked;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.CharacterStats.OnAttacked -= OnAttacked;
        }


        protected override void OnAttacked(DamageInfo info)
        {
            var source = info.Self;

            // 攻擊者有燃燒狀態
            if (source != null)
            {
                if (source.HasPower(PowerType.Fire))
                {
                    // 給予能力持有者回合開始時，獲得瑪娜的能力
                    var parameter = new ApplyPowerParameters(info.Target, 
                        PowerType.GainManaAtRoundStart, 1);
                    GameActionExecutor.DoApplyPowerAction(parameter);
                }
            }
        }
    }
}