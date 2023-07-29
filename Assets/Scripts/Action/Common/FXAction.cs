using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Enums;

namespace NueGames.Action
{
    public class FXAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.FX;

        public FXAction(FxName fxName,  FxSpawnPosition fxSpawnPosition, List<CharacterBase> targetList)
        {
            SetFXValue(fxName, fxSpawnPosition);
            Parameters.TargetList = targetList;
        }
        
        protected override void DoMainAction()
        {
            
        }
    }
}