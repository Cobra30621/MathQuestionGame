using Managers;

namespace NueGames.Event.Effect
{
    public class ChangeHealthEffect : IEffect
    {
        private int changeHealthValue;
        
        public void Init(EffectData effectData)
        {
            changeHealthValue = effectData.changeHealthValue;
        }

        public void Execute(System.Action onComplete)
        {
           GameManager.Instance.AllyHealthHandler.AddHealth(changeHealthValue);
           
           onComplete?.Invoke();
        }

        public bool IsSelectable()
        {
            return true;
        }
    }
}