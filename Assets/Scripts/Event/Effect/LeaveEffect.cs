namespace NueGames.Event.Effect
{
    public class LeaveEffect : IEffect
    {
        public void Init(EffectData effectData)
        {
            
        }

        public void Execute()
        {
            EventManager.LeaveEventSystem();
        }

        public bool IsSelectable()
        {
            return true;
        }
    }
}