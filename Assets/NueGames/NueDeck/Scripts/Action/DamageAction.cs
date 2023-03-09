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
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            damageInfo = new DamageInfo(data.ActionValue, cardActionParameter.SelfCharacter);
            TargetCharacter = cardActionParameter.TargetCharacter;
            Duration = cardActionParameter.CardActionData.ActionDelay;
        }
        
        public void SetValue(DamageInfo damageInfo, CharacterBase target)
        {
            this.damageInfo = damageInfo;
            TargetCharacter = target;
        }
        
        public override void DoAction()
        {
            if (!TargetCharacter) return;

            int value = CombatCalculator.GetDamageValue(damageInfo.Value, damageInfo.SelfCharacter, TargetCharacter);
            
            TargetCharacter.CharacterStats.Damage(value);

            if (FxManager != null)
            {
                FxManager.PlayFx(TargetCharacter.transform,FxType.Attack);
                FxManager.SpawnFloatingText(TargetCharacter.TextSpawnRoot,value.ToString());
            }
           
            PlayAudio();
        }
    }
}