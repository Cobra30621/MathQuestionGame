using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        private DamageInfo damageInfo;
        private PowerType accordingPowerType;
        private CardActionParameter cardParameter;
        
        public DamageByAllyPowerValueAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            cardParameter = cardActionParameter;
            CardActionData data = cardActionParameter.CardActionData;
            damageInfo = new DamageInfo(cardActionParameter.Value, cardActionParameter.SelfCharacter);
            Target = cardActionParameter.TargetCharacter;
            Duration = cardActionParameter.CardActionData.ActionDelay;
            accordingPowerType = data.PowerType;
        }
        
        public override void DoAction()
        {
            Value = CombatManager.CurrentMainAlly.GetPowerValue(accordingPowerType);
            CardActionParameter newParameter = new CardActionParameter(
                Value, 
                cardParameter.TargetCharacter, 
                cardParameter.SelfCharacter, 
                cardParameter.CardActionData, 
                cardParameter.CardData
            );
            GameActionBase gameActionBase = GameActionManager.GetGameAction(GameActionType.Damage, newParameter);
            GameActionManager.AddToBottom(gameActionBase);
        }
    }
}