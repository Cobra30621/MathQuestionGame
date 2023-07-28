using NueGames.Enums;

namespace NueGames.Action
{
    public class FXAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.FX;
        
        
        public void SetValue(FxName fxName,  FxSpawnPosition fxSpawnPosition)
        {
            SetFXValue(fxName, fxSpawnPosition);
        }
        
        protected override void DoMainAction()
        {
            
        }
    }
}