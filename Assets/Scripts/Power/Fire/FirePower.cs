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

        public FirePower()
        {
            DecreaseOverTurn = true;
        }


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
                DamageInfo damageInfo = new DamageInfo()
                {
                    Value = Value,
                    Target = Owner,
                    FixDamage = true
                };

                DamageAction damageAction = new DamageAction();
                damageAction.SetValue(damageInfo);
                
                GameActionExecutor.AddToBottom(damageAction);
            }
        }
    }
}