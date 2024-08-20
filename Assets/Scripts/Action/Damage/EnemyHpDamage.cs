using Card;

namespace NueGames.Action
{
    public class EnemyHpDamage : GameActionBase
    {
        private int _damage;
        private int _times;
        
        public EnemyHpDamage(int times)
        {
            _times = times;
        }

        public EnemyHpDamage(SkillInfo skillInfo)
        {
            _times = skillInfo.EffectParameterList[0];
        }
        
        protected override void DoMainAction()
        {
            // 計算怪物總血量
            _damage = 1;
            var multiDamageAction = new MultiDamageAction(_damage, _times);
            multiDamageAction.DoAction();
        }
    }
}