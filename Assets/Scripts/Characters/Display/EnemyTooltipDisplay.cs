using UnityEngine;

namespace Characters.Display
{
    public class EnemyTooltipDisplay : CharacterTooltipDisplay
    {
        [SerializeField] private Enemy.Enemy _enemy;
        
        public override void ShowTooltipInfo()
        {
            if (_enemy == null)
            {
                return;
            }

            var skill = _enemy.currentSkill;
            if (skill == null)
            {
                return;
            }
            
            var intention = skill._intention;
            ShowTooltipInfo(intention.Content, intention.Header);
            
            ShowPowerTooltipInfo();
        }
    }
}