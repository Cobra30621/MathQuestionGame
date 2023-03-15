using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Action
{
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        private DamageInfo damageInfo;
        private PowerType accordingPowerType;
        
        public DamageByAllyPowerValueAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            damageInfo = new DamageInfo(parameters.Value, parameters.SelfCharacter);
            Duration = parameters.CardActionData.ActionDelay;

            SetValue(new DamageInfo(parameters.Value, parameters.SelfCharacter),
                parameters.TargetCharacter, data.PowerType);
        }

        public void SetValue(DamageInfo info, CharacterBase target, PowerType accordingPower)
        {
            damageInfo = info;
            Target = target;
            accordingPowerType = accordingPower;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            damageInfo.Value  = CombatManager.CurrentMainAlly.GetPowerValue(accordingPowerType);
            
            DamageAction gameActionBase = new DamageAction();
            gameActionBase.SetValue(damageInfo, Target);
            GameActionExecutor.AddToBottom(gameActionBase);
        }
    }
}