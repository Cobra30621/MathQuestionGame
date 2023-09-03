using System.Collections.Generic;
using Action.Parameters;
using NueGames.Characters;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    public class FXAction : GameActionBase
    {
        private FxInfo _fxInfo;

        public FXAction(FxInfo fxInfo, List<CharacterBase> targetList)
        {
            _fxInfo = fxInfo;
            TargetList = targetList;
        }
        
        
        protected override void DoMainAction()
        {
            Debug.Log($"_fxInfo.FxGo{_fxInfo.FxGo}");
            // 不播放特效
            if (_fxInfo.FxGo == null)
            {
                return;
            }

            var spawnTransform = FxManager.GetFXSpawnPosition(_fxInfo.FxSpawnPosition);
            switch (_fxInfo.FxSpawnPosition)
            {
                case FxSpawnPosition.EachTarget:
                    foreach (var target in TargetList)
                    {
                        FxManager.PlayFx(_fxInfo.FxGo, spawnTransform, target.transform.position);
                    };
                    break;
                case FxSpawnPosition.Ally:
                    spawnTransform.position = CombatManager.GetMainAllyTransform().position;
                    FxManager.PlayFx(_fxInfo.FxGo, spawnTransform);
                    break;
                case FxSpawnPosition.EnemyMiddle:
                case FxSpawnPosition.ScreenMiddle:
                    FxManager.PlayFx(_fxInfo.FxGo, spawnTransform);
                    break;
            }
        }
    }
}