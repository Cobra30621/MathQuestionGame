using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Action
{
    public abstract class GameActionBase
    {
        protected bool HasSetValue = false;
        protected CharacterBase Target;
        protected CharacterBase Self;
        protected int Amount;
        public float Duration = 0;

        protected Transform FXTransform;
        public FxType FxType;
        public AudioActionType AudioActionType;

        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected GameActionExecutor GameActionExecutor => GameActionExecutor.Instance;


        public virtual void SetValue(CardActionParameters parameters){}

        public abstract void DoAction();
        

        protected void PlaySpawnTextFx()
        {
            if (Target != null)
            {
                FxManager.SpawnFloatingText(Target.TextSpawnRoot,Amount.ToString());
            }
        }
        
        protected void PlayFx()
        {
            // TODO 設定 FX 預設產生處
            if (Target != null)
            {
                FxManager.PlayFx(Target.transform, FxType);
            }
                
        }

        protected void PlayAudio()
        {
            AudioManager.PlayOneShot(AudioActionType);
        }
        

        // ReSharper disable Unity.PerformanceAnalysis
        protected void CheckHasSetValue()
        {
            if (!HasSetValue)
            {
                Debug.LogError($"{this.GetType()} hasn't set value.\n Please Call SetValue");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected bool IsTargetNull()
        {
            if (!Target)
            {
                Debug.Log($"{GetType()} 找不到Target");
            }

            return !Target;
        }
    }
}