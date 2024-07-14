using System;
using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Action.Sequence
{
    [Serializable]
    public class FXSequence : ISequence
    {
        [ShowInInspector]
        private FxInfo _fxInfo;
        [ShowInInspector]
        private List<GameActionBase> _actions;
        [ShowInInspector]
        protected List<CharacterBase> TargetList;
        
        protected FxManager FxManager => FxManager.Instance;

        [ShowInInspector]
        private List<FXPlayer> playingFXs;


        public FXSequence( List<GameActionBase> gameAction, FxInfo fxInfo, List<CharacterBase> targetList)
        {
            _fxInfo = fxInfo;
            _actions = gameAction;
            TargetList = targetList;
        }


        public override IEnumerator Execute(System.Action onComplete)
        {
            foreach (var action in _actions)
            {
                Debug.Log(action);
                action.DoAction();
            }
            
            playingFXs = new List<FXPlayer>();
            FXPlayer fxPlayer;
            
            if (_fxInfo.FxGo != null)
            {
                var spawnTransform = FxManager.GetFXSpawnPosition(_fxInfo.FxSpawnPosition);
                switch (_fxInfo.FxSpawnPosition)
                {
                    case FxSpawnPosition.EachTarget:
                        foreach (var target in TargetList)
                        {
                            fxPlayer = FxManager.PlayFx(_fxInfo.FxGo, spawnTransform, 
                                target.transform.position);
                            playingFXs.Add(fxPlayer);
                        };
                        break;
                    case FxSpawnPosition.Ally:
                        spawnTransform.position = CombatManager.Instance.GetMainAllyTransform().position;
                        fxPlayer = FxManager.PlayFx(_fxInfo.FxGo, spawnTransform);
                        playingFXs.Add(fxPlayer);
                        break;
                    case FxSpawnPosition.EnemyMiddle:
                    case FxSpawnPosition.ScreenMiddle:
                        fxPlayer =FxManager.PlayFx(_fxInfo.FxGo, spawnTransform);
                        playingFXs.Add(fxPlayer);
                        break;
                }
            }

            Debug.Log($"Wait");
            yield return WaitComplete();
            
            onComplete.Invoke();
        }

        private IEnumerator WaitComplete()
        {
            if (_fxInfo.WaitMethod == WaitMethod.WaitFXFinish)
            {
                yield return new WaitUntil(()=> !HaveFXPlaying());
            }else if (_fxInfo.WaitMethod == WaitMethod.WaitDelay)
            {
                yield return new WaitForSeconds(_fxInfo.Delay);
            }
        }

        private bool HaveFXPlaying()
        {
            foreach (var playingFX in playingFXs)
            {
                if (playingFX.IsPlaying())
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(_fxInfo)}: {_fxInfo}, {nameof(_actions)}: {_actions}, {nameof(TargetList)}: {TargetList}, {nameof(playingFXs)}: {playingFXs}";
        }
    }
}