namespace Action.Damage
{
    
    public class EnemyHpDamageAction: GameActionBase
    {
        private int _times;
        
        /// <summary>
        /// 讀表
        /// </summary>
        /// <param name="skillInfo"></param>
        public EnemyHpDamageAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _times = skillInfo.EffectParameterList[0];
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            var totalHealth = CombatManager.GetEnemyTotalHealth();
            
            var damageAction = new MultiDamageAction(
                totalHealth, _times, TargetList, ActionSource);
            damageAction.DoAction();
        }

        public override (int, int) GetDamageBasicInfo()
        {
            return (CombatManager.GetEnemyTotalHealth(), _times);
        }
    }
}