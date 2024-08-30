using System;
using Combat;
using Enemy;
using NueGames.Enums;

namespace NueGames.Combat
{
    public class CharacterHighlightController
    {
        private static CombatManager CombatManager => CombatManager.Instance;


        public void ActivateEnemyHighlight(EnemyBase enemyBase)
        {
            enemyBase.EnemyCanvas.SetHighlight(true);
        }
        
        public void OnDraggedCardOutsideHand(ActionTargetType targetTypeTargetType)
        {
            switch (targetTypeTargetType)
            {
                case ActionTargetType.AllEnemies:
                case ActionTargetType.RandomEnemy:
                    ActivateAllEnemyHighlights();
                    break;
                case ActionTargetType.Ally:
                    ActivateAllAllyHighlights();
                    break;
                case ActionTargetType.SpecifiedEnemy:
                case ActionTargetType.WithoutTarget:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetTypeTargetType), targetTypeTargetType, null);
            }
        }

        void ActivateAllEnemyHighlights()
        {
            foreach (var currentEnemy in CombatManager.Enemies)
                currentEnemy.EnemyCanvas.SetHighlight(true);
        }

        void ActivateAllAllyHighlights()
        {
            CombatManager.MainAlly.AllyCanvas.SetHighlight(true);
        }
        
        public void DeactivateAllHighlights()
        {
            DeactivateAllyHighlights();
            DeactivateEnemyHighlights();
        }
        
        public void DeactivateAllyHighlights()
        {
            CombatManager.MainAlly.AllyCanvas.SetHighlight(false);
        }
        
        public void DeactivateEnemyHighlights()
        {
            foreach (var currentEnemy in CombatManager.Enemies)
                currentEnemy.EnemyCanvas.SetHighlight(false);
        }
    }
}

