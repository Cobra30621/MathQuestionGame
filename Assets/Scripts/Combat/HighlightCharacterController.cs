using System;
using Characters.Enemy;

namespace Combat
{
    public class CharacterHighlightController
    {
        private static CombatManager CombatManager => CombatManager.Instance;


        public void ActivateEnemyHighlight(Enemy enemy)
        {
            enemy.CharacterCanvas.SetHighlight(true);
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
                currentEnemy.CharacterCanvas.SetHighlight(true);
        }

        void ActivateAllAllyHighlights()
        {
            CombatManager.MainAlly.CharacterCanvas.SetHighlight(true);
        }
        
        public void DeactivateAllHighlights()
        {
            DeactivateAllyHighlights();
            DeactivateEnemyHighlights();
        }
        
        public void DeactivateAllyHighlights()
        {
            CombatManager.MainAlly.CharacterCanvas.SetHighlight(false);
        }
        
        public void DeactivateEnemyHighlights()
        {
            foreach (var currentEnemy in CombatManager.Enemies)
                currentEnemy.CharacterCanvas.SetHighlight(false);
        }
    }
}

