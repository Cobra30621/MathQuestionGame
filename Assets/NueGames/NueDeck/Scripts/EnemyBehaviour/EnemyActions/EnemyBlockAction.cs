using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.EnemyBehaviour.EnemyActions
{
    public class EnemyBlockAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Block;
        
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            
            var newTarget = actionParameters.TargetCharacter
                ? actionParameters.TargetCharacter
                : actionParameters.SelfCharacter;
            
            if (!newTarget) return;
            
            newTarget.CharacterStats.ApplyPower(PowerType.Block,
                Mathf.RoundToInt(actionParameters.Value + actionParameters.SelfCharacter.CharacterStats
                    .PowerDict[PowerType.Block].Value));
            
            if (FxManager != null)
                FxManager.PlayFx(newTarget.transform,FxType.Block);

            if (AudioManager != null)
                AudioManager.PlayOneShot(AudioActionType.Block);
        }
    }
}