namespace Effect.Other
{
    public class EndPlayerTurnEffect : EffectBase
    {
        
        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public EndPlayerTurnEffect(SkillInfo skillInfo)
        {
            
        }
        
        public override void Play()
        {
            CombatManager.EndTurn();
        }
    }
}