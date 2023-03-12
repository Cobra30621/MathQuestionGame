using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.EnemyBehaviour.EnemyActions
{
    public class EnemyGiveStatusAction : EnemyActionBase
    {
        public override EnemyActionType ActionType => EnemyActionType.GiveStatus;
        public override void DoAction(EnemyActionParameters actionParameters)
        {
            var newTarget = actionParameters.TargetCharacter;

            if (!newTarget) return;

            PowerType powerType = actionParameters.ActionData.PowerType;
            newTarget.CharacterStats.ApplyPower(powerType,Mathf.RoundToInt(actionParameters.Value));
            
            if (FxManager != null) 
                FxManager.PlayFx(newTarget.transform, FxType.Poison);
            
            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType.Poison);
        }
    }
}