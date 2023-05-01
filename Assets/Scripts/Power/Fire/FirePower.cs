using NueGames.Action;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Power
{
    /// <summary>
    /// 燃燒能力
    /// </summary>
    public class FirePower : PowerBase
    {
        public override PowerType PowerType => PowerType.Fire;
        
        protected override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        protected override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }


        protected override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == GetOwnerCharacterType())
            {
                DamageInfo damageInfo = GetDamageInfo( Amount, true, true);
                DoDamageAction(damageInfo);
                Owner.CharacterStats.ApplyPower(PowerType, -1); // 燒血後減層數 1 
            }
        }
    }
}