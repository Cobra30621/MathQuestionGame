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
        public CharacterBase Target;
        public CharacterBase Self;
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
        protected GameActionManager GameActionManager => GameActionManager.Instance;


        public virtual void SetValue(CardActionParameter cardActionParameter){}

        public abstract void DoAction();


        public void AddToTop()
        {
            AddToTop(this);
        }

        public void AddToBottom()
        {
            AddToBottom(this);
        }
        
        public void AddToTop(GameActionBase gameActionBase)
        {
            GameActionManager.AddToTop(gameActionBase);
        }

        public void AddToBottom(GameActionBase gameActionBase)
        {
            GameActionManager.AddToTop(gameActionBase);
        }
        

        protected void PlayFx()
        {
            // TODO 設定 FX 預設產生處
            if (FxManager != null)
            {
                if(Target != null)
                    FxManager.PlayFx(Target.transform, FxType);
            }
                
        }

        protected void PlayAudio()
        {
            if (AudioManager != null) 
                AudioManager.PlayOneShot(AudioActionType);
        }

        protected void PrintInfo()
        {
            Debug.Log($"{this.GetType()} \n " +
                      $"value{Value}");
        }
    }
}