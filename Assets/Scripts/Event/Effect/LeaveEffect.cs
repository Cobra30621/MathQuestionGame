namespace NueGames.Event.Effect
{
    public class LeaveEffect : IEffect
    {
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