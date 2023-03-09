using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public abstract class GameActionBase
    {
        public GameActionType ActionType { get;}
        public CharacterBase TargetCharacter;
        public CharacterBase SelfCharacter;
        public int Value;
        public float Duration = 0;

        protected Transform fxTransform;
        public FxType FxType;
        public AudioActionType AudioActionType;
        
        public CardActionData CardActionData;

        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;


        public virtual void SetValue(CardActionParameter cardActionParameter){}

        public abstract void DoAction();

        protected void PlayFx()
        {
            // TODO 設定 FX 預設產生處
            if (FxManager != null)
            {
                if(TargetCharacter != null)
                    FxManager.PlayFx(TargetCharacter.transform, FxType);
            }
                
        }

        protected void PlayAudio()
        {
            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType);
        }
    }
}