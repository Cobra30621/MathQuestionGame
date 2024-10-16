using Combat;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{
    public class BloodFuryPower : PowerBase
    {
        public override PowerName PowerName => PowerName.BloodFury;
        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            // if (info.CharacterType == GetOwnerCharacterType())
            // {
            //     ClearPower();
            // }
        }
        public override float AtDamageReceive(float damage)
        {
            return damage * 1.3f;
        }
        public override float AtDamageGive(float damage)
        {
            return damage * 2;
        }
    }
}