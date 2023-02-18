using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Card.CardActions
{
    public class AttackSelfAction :  CardActionBase
    {
        public override CardActionType ActionType => CardActionType.AttackSelf;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (!actionParameters.TargetCharacter) return;

            var targetCharacter = actionParameters.TargetCharacter;
            var selfCharacter = actionParameters.SelfCharacter;
            var value = CombatCalculator.GetDamageValue(actionParameters.Value, selfCharacter, targetCharacter);
            
            actionParameters.SelfCharacter.CharacterStats.Damage(value);
            
            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.SelfCharacter.transform,FxType.Attack);
                FxManager.SpawnFloatingText(actionParameters.SelfCharacter.TextSpawnRoot,value.ToString());
            }
           
            if (AudioManager != null) 
                AudioManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}


