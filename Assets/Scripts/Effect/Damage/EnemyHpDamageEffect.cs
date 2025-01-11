namespace Effect.Damage
{
    
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
        protected override void DoMainAction()
        {
            var totalHealth = CombatManager.GetEnemyTotalHealth();
            
            var damageAction = new MultiDamageEffect(
                totalHealth, _times, TargetList, ActionSource);
            damageAction.DoAction();
        }

        public override (int, int) GetDamageBasicInfo()
        {
            return (CombatManager.GetEnemyTotalHealth(), _times);
        }
    }
}