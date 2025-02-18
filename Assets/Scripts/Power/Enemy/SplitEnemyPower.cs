namespace Power.Enemy
{
    /// <summary>
    /// 分裂敌人能力类
    /// </summary>
    public class SplitEnemyPower : PowerBase
    {
        /// <summary>
        /// 能力名称
        /// </summary>
        public override PowerName PowerName => PowerName.SplitEnemy;


        /// <summary>
        /// 处理生命值变化的方法
        /// </summary>
        /// <param name="health">当前生命值</param>
        /// <param name="maxHealth">最大生命值</param>
        public override void OnHealthChanged(int health, int maxHealth)
        {
            // 当生命值低于一半时
            if ((float)health / maxHealth < 0.5f)
            {
                var enemy = (global::Characters.Enemy.Enemy)Owner;
                // 设置敌人为分裂状态
                enemy.SetSplitEnemySkill($"{Amount}");
                
                // 清除当前能力
                Owner.ClearPower(PowerName, GetEffectSource());
            }
        }
    }
}
