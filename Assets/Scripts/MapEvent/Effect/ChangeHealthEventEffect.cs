using Managers;
using MapEvent.Data;

namespace MapEvent.Effect
{
    public class ChangeHealthEventEffect : IEventEffect
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