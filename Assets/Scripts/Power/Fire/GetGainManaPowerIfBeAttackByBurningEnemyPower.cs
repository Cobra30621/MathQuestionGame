using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;

namespace NueGames.Power
{
    /// <summary>
    /// 火舞: 遭受到來自燃燒狀態的敵人攻擊時 下一回合魔力額外增加1 
    /// </summary>
    public class GetGainManaPowerIfBeAttackByBurningEnemyPower : PowerBase
    {
        public override PowerName PowerName => PowerName.GetGainManaPowerIfBeAttackByBurningEnemy;


        public override void SubscribeAllEvent()
        {
            Owner.OnAttacked += OnAttacked;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.OnAttacked -= OnAttacked;
        }


        protected override void OnAttacked(DamageInfo info)
        {;
            var source = info.ActionSource.SourceCharacter;

            // 攻擊者有燃燒狀態
            if (source != null)
            {
                if (source.HasPower(PowerName.Fire))
                {
                    GameActionExecutor.AddToBottom (
                        new ApplyPowerAction(1, PowerName.GainManaAtRoundStart, 
                        new List<CharacterBase>(){info.Target}, GetActionSource()));
                }
            }
        }
    }
}