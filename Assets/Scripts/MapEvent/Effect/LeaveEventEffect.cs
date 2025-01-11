using MapEvent.Data;

namespace MapEvent.Effect
{
    public class LeaveEventEffect : IEventEffect
    {
        public void Init(EffectData effectData)
        {
            
        }

        public void Execute(System.Action onComplete)
        {
            EventManager.Instance.LeaveEventSystem();
            
            onComplete.Invoke();
        }

        public bool IsSelectable()
        {
            return true;
        }
    }
}