namespace NueGames.Event.Effect
{
    public class LeaveEffect : IEffect
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