using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 增加最大生命值
    /// </summary>
    public class IncreaseMaxHealthAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.IncreaseMaxHealth;
        public IncreaseMaxHealthAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;
            
            SetValue(data.ActionValue,parameters.TargetCharacter);
        }

        public void SetValue(int increaseValue, CharacterBase target)
        {
            Amount = increaseValue;
            Target = target;

            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.CharacterStats.IncreaseMaxHealth(Mathf.RoundToInt(Amount));

            if (FxManager != null)
            {
                FxManager.PlayFx(Target.transform,FxType.Attack);
                FxManager.SpawnFloatingText(Target.TextSpawnRoot,Amount.ToString());
            }
            PlayAudio();
        }
    }
}