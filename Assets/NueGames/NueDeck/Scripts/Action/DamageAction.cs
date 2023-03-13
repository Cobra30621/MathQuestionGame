using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class DamageAction : GameActionBase
    {
        private DamageInfo damageInfo;
        
        public DamageAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardActionData data = parameters.CardActionData;
            Duration = parameters.CardActionData.ActionDelay;
            
            SetValue(new DamageInfo(parameters.Value, parameters.SelfCharacter),
                parameters.TargetCharacter);
        }
        
        public void SetValue(DamageInfo info, CharacterBase target)
        {
            damageInfo = info;
            Target = target;

            hasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;

            int value = CombatCalculator.GetDamageValue(damageInfo.Value, damageInfo.SelfCharacter, Target);
            
            Target.CharacterStats.Damage(value);

            if (FxManager != null)
            {
                FxManager.PlayFx(Target.transform,FxType.Attack);
                FxManager.SpawnFloatingText(Target.TextSpawnRoot,value.ToString());
            }
           
            PlayAudio();
        }
    }
}