using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;

namespace NueGames.Action.MathAction
{
    public class DamageByQuestioningAction : ByQuestioningActionBase
    {
        private DamageInfo damageInfo;
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;

            SetValue(new DamageInfo(data.ActionValue, data.AdditionValue, parameters.SelfCharacter),
                parameters.TargetCharacter);
        }
        
        public void SetValue(DamageInfo info, CharacterBase target)
        {
            damageInfo = info;
            Target = target;
            baseValue = info.Value;
            additionValue = info.AdditionalValue;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            damageInfo.Value  = GetAddedValue();
            
            DamageAction gameActionBase = new DamageAction();
            gameActionBase.SetValue(damageInfo, Target);
            GameActionExecutor.AddToBottom(gameActionBase);
        }

    }
}