using NueGames.Action;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 燃燒能力
    /// </summary>
    public class FirePower : PowerBase
    {
        public override PowerType PowerType => PowerType.Fire;
        
        public override void SubscribeAllEvent()
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

                Debug.Log( "Fire" +  damageInfo);
                GameActionExecutor.DoDamageAction(damageInfo);
                
                Owner.CharacterStats.ApplyPower(PowerType, -1); // 燒血後減層數 1 
            }
        }
    }
}