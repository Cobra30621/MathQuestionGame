using Card;
using Combat;
using UnityEngine;

namespace NueGames.Action
{
    public class EnemyCountBased : GameActionBase
    {
        private int count;
        private int type;
        
        public EnemyCountBased(int _type)
        {
            type = _type;
        }

        public EnemyCountBased(SkillInfo skillInfo)
        {
            type = skillInfo.EffectParameterList[0];
        }
        
        protected override void DoMainAction()
        {
            // 計算敵人數量
            var allEnemy = CombatManager.Instance.Enemies;
            count = allEnemy.Count;
            // 造成傷害
            if (type == 1)
            {
                // 這裡刪掉改成隕石
                // 計算傷害
                int damage = count;
                var multiDamageAction = new MultiDamageAction(damage, 1);
                multiDamageAction.DoAction();
            }
            else if (type == 2)
            {
                
            }
            else Debug.LogError("Invalid type");
            
        }
    }
}