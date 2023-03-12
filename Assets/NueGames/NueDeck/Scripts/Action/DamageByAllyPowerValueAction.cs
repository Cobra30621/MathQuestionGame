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
        private CardActionParameters cardParameters;
        
        public DamageByAllyPowerValueAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameters cardActionParameters)
        {
            cardParameters = cardActionParameters;
            CardActionData data = cardActionParameters.CardActionData;
            damageInfo = new DamageInfo(cardActionParameters.Value, cardActionParameters.SelfCharacter);
            Target = cardActionParameters.TargetCharacter;
            Duration = cardActionParameters.CardActionData.ActionDelay;
            accordingPowerType = data.PowerType;
        }
        
        public override void DoAction()
        {
            Value = CombatManager.CurrentMainAlly.GetPowerValue(accordingPowerType);
            CardActionParameters newParameters = new CardActionParameters(
                Value, 
                cardParameters.TargetCharacter, 
                cardParameters.SelfCharacter, 
                cardParameters.CardActionData, 
                cardParameters.CardData
            );
            GameActionBase gameActionBase = GameActionManager.GetGameAction(GameActionType.Damage, newParameters);
            GameActionManager.AddToBottom(gameActionBase);
        }
    }
}