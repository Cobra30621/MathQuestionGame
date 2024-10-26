using Enemy;

namespace NueGames.Power
{
    public class KingSmileSplitPower : PowerBase
    {
        public override PowerName PowerName => PowerName.KingSmileSplit;
        
        
        
        
        /// <summary>
        /// 订阅所有事件
        /// </summary>
        public override void SubscribeAllEvent()
        {
            Owner.OnHealthChanged += HealthChanged;
        }

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        public override void UnSubscribeAllEvent()
        {
            Owner.OnHealthChanged -= HealthChanged;
        }

        /// <summary>
        /// 处理生命值变化的方法
        /// </summary>
        /// <param name="health">当前生命值</param>
        /// <param name="maxHealth">最大生命值</param>
        public void HealthChanged(int health, int maxHealth)
        {
            // 当生命值低于一半时
            if ((float)health / maxHealth < 0.5f)
            {
                var enemy = (EnemyBase)Owner;
                // 设置敌人为分裂状态，並獲得黏液能力
                enemy.SetSplitEnemySkillAndGainSmilePower();
                
                // 清除当前能力
                Owner.ClearPower(PowerName);
            }
        }
    }
}