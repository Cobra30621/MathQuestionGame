using Combat;
using GameListener;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{
    public class MissPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Miss;
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
        public override float AtDamageGive(float damage)
        {
            return damage * 0;
        }

        public MissPower()
        {
            DamageCalculateOrder = CalculateOrder.FinalChange;
        }
    }
}