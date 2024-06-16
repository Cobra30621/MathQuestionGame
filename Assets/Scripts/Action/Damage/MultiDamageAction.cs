using Action.Parameters;
using Card;
using NueGames.Managers;

namespace NueGames.Action
{
    public class MultiDamageAction : GameActionBase
    {
        /// <summary>
        /// 傷害值
        /// </summary>
        private int _damage;
        
        /// <summary>
        /// 傷害次數
        /// </summary>
        private int _times;
        

        public MultiDamageAction(int damage, int times)
        {
            _damage = damage;
            _times = times;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public MultiDamageAction(SkillInfo skillInfo)
        {
            _damage = skillInfo.int1;
            _times = skillInfo.int2;
        }

        protected override void DoMainAction()
        {
            for (int i = 0; i < _times; i++)
            {
                var damageInfo = new DamageInfo(_damage, ActionSource);
                var damageAction = new DamageAction(damageInfo, TargetList);
         
                GameActionExecutor.AddToBottom(damageAction);
            }
        }
    }
}