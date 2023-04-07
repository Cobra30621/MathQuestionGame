using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;

namespace NueGames.Action
{
    /// <summary>
    /// 根據玩家的能力層數，給予傷害
    /// </summary>
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByAllyPowerValue;
        private DamageInfo damageInfo;
        private PowerType accordingPowerType;
        
        public DamageByAllyPowerValueAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            damageInfo = new DamageInfo(parameters.Value, parameters.SelfCharacter);
            Duration = parameters.ActionData.ActionDelay;

            SetValue(new DamageInfo(parameters.Value, parameters.SelfCharacter),
                parameters.TargetCharacter, data.PowerType);
        }

        public void SetValue(DamageInfo info, CharacterBase target, PowerType accordingPower)
        {
            damageInfo = info;
            Target = target;
            accordingPowerType = accordingPower;

            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            damageInfo.Value  = CombatManager.CurrentMainAlly.GetPowerValue(accordingPowerType);
            
            DamageAction gameActionBase = new DamageAction();
            gameActionBase.SetValue(damageInfo, Target);
            GameActionExecutor.AddToBottom(gameActionBase);
        }
    }
}