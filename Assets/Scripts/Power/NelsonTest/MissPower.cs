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
            CombatManager.Instance.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.Instance.OnTurnStart -= OnTurnStart;
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