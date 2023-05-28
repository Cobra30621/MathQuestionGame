using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 播放特效的行動
    /// </summary>
    public class FXAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.FX;
        public override void DoAction()
        {
            PlayFx(FxType, GetFXSpawnPosition(FxSpawnPosition));
        }

        private Transform GetFXSpawnPosition(FxSpawnPosition fxSpawnPosition)
        {
            switch (fxSpawnPosition)
            {
                case FxSpawnPosition.Ally:
                    return CombatManager.GetMainAllyTransform();
                case FxSpawnPosition.Middle:
                    return null;
                case FxSpawnPosition.Target:
                    return Target.transform;
            }

            return null;
        }
    }
}