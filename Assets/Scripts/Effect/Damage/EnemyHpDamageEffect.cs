using Effect.Parameters;

namespace Effect.Damage
{
    /// <summary>
    /// 造成怪物生命總合的傷害[1]次
    /// </summary>
    public class EnemyHpDamageEffect: EffectBase
    {
        private int _times;
        
        /// <summary>
        /// 讀表
        /// </summary>
        /// <param name="skillInfo"></param>
        public EnemyHpDamageEffect(SkillInfo skillInfo)
        {
            _times = skillInfo.EffectParameterList[0];
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void Play()
        {
            var totalHealth = CombatManager.GetEnemyTotalHealth();

            var damageInfo = new DamageInfo(totalHealth, EffectSource);
            var damageEffect = new DamageEffect(
                damageInfo,  TargetList, times:_times);
            damageEffect.Play();
        }

        public override (int, int) GetDamageBasicInfo()
        {
            return (CombatManager.GetEnemyTotalHealth(), _times);
        }
    }
}