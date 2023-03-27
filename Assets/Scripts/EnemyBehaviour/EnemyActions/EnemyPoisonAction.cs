using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.EnemyBehaviour.EnemyActions
{
    public class EnemyPoisonAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.Poison;
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter;

            if (!newTarget) return;
            
            newTarget.CharacterStats.ApplyPower(PowerType.Poison,Mathf.RoundToInt(actionParameters.Value));
            
            if (FxManager != null) 
                FxManager.PlayFx(newTarget.transform, FxType.Poison);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType.Poison);
        }
    }
}