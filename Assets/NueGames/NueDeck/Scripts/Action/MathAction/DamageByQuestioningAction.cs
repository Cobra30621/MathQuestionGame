using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;

namespace NueGames.NueDeck.Scripts.Action.MathAction
{
    public class DamageByQuestioningAction : ByQuestioningActionBase
    {
        private DamageInfo damageInfo;
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;

            SetValue(new DamageInfo(data.ActionValue, data.AdditionValue, parameters.SelfCharacter),
                parameters.TargetCharacter);
        }
        
        public void SetValue(DamageInfo info, CharacterBase target)
        {
            damageInfo = info;
            Target = target;
            baseValue = info.Value;
            additionValue = info.AddtionValue;

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