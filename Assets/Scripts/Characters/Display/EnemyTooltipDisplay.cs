using UnityEngine;

namespace Characters.Display
{
    public class EnemyTooltipDisplay : CharacterTooltipDisplay
    {
        [SerializeField] private Enemy.Enemy _enemy;
        
        public override void ShowTooltipInfo()
        {
            ShowPowerTooltipInfo();
            
            var intention = _enemy.currentSkill._intention;
            ShowTooltipInfo(intention.Content, intention.Header);
        }
    }
}