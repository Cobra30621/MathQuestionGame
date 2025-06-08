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
            var intention = _enemy.currentSkill._intention;
            ShowTooltipInfo(intention.Content, intention.Header);
            
            ShowPowerTooltipInfo();
        }
    }
}