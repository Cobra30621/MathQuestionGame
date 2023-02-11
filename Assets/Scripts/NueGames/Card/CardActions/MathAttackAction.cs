using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.Card.CardActions;
using UnityEngine;
using Question;

namespace NueGames.Card.CardActions
{
    public class MathAttackAction: MathActionBase
    {
        public override CardActionType ActionType => CardActionType.MathAttack;
        public CardActionParameters actionParameters;
        public override void DoAction(CardActionParameters actionParameters)
        {
            QuestionManager.Instance.EnterQuestion(this);
            this.actionParameters = actionParameters;
        }

        public override void OnAnswer()
        {
            if (!actionParameters.TargetCharacter) return;
            
            var targetCharacter = actionParameters.TargetCharacter;
            var selfCharacter = actionParameters.SelfCharacter;
            
            var value = actionParameters.Value + selfCharacter.CharacterStats.StatusDict[StatusType.Strength].StatusValue; 
            
            targetCharacter.CharacterStats.Damage(Mathf.RoundToInt(value));

            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.TargetCharacter.transform,FxType.Attack);
                FxManager.SpawnFloatingText(actionParameters.TargetCharacter.TextSpawnRoot,value.ToString());
            }
           
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}