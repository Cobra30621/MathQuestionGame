using Combat;
using GameListener;

namespace NueGames.Power
{
    public class SmilePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Smile;


        public SmilePower()
        {
            DamageCalculateOrder = CalculateOrder.AdditionAndSubtraction;
        }
        

        public override float AtDamageGive(float damage)
        {
            var totalHealth = 0;
            foreach (var enemy in CombatManager.Instance.Enemies)
            {
                totalHealth += enemy.GetHealth();
            }

            return damage + totalHealth;

        }
    }
}