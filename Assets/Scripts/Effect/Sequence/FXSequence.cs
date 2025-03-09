using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Combat;
using Effect.Parameters;
using Feedback;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Sequence
{
    [Serializable]
    public class FXSequence : ISequence
    {
        [ShowInInspector]
        private FxInfo _fxInfo;
        [ShowInInspector]
        private List<EffectBase> _effects;
        [ShowInInspector]
        protected List<CharacterBase> TargetList;
        
        protected FxManager FxManager => FxManager.Instance;

        [ShowInInspector]
        private List<FXPlayer> playingFXs;

        private Action _onCompleted;


        public FXSequence( List<EffectBase> effects, FxInfo fxInfo, List<CharacterBase> targetList, Action onComplete)
        {
            _fxInfo = fxInfo;
            _effects = effects;
            TargetList = targetList;
            _onCompleted = onComplete;
        }


        public override IEnumerator Execute(Action setActionCompleted)
        {
            foreach (var effect in _effects)
            {
                effect.Play();
            }
            
            playingFXs = new List<FXPlayer>();
            FXPlayer fxPlayer;
            
            if (_fxInfo.FxPrefab != null)
            {
                var spawnTransform = FxManager.GetFXSpawnPosition(_fxInfo.FxSpawnPosition);
                switch (_fxInfo.FxSpawnPosition)
                {
                    case FxSpawnPosition.EachTarget:
                        foreach (var target in TargetList)
                        {
                            fxPlayer = FxManager.PlayFx(_fxInfo.FxPrefab, spawnTransform, 
                                target.transform.position);
                            playingFXs.Add(fxPlayer);
                            fxPlayer.Play();
                        };
                        break;
                    case FxSpawnPosition.Ally:
                        spawnTransform.position = CombatManager.Instance.GetMainAllyTransform().position;
                        fxPlayer = FxManager.PlayFx(_fxInfo.FxPrefab, spawnTransform);
                        fxPlayer.Play();
                        playingFXs.Add(fxPlayer);
                        break;
                    case FxSpawnPosition.EnemyMiddle:
                    case FxSpawnPosition.ScreenMiddle:
                        fxPlayer =FxManager.PlayFx(_fxInfo.FxPrefab, spawnTransform);
                        fxPlayer.Play();
                        playingFXs.Add(fxPlayer);
                        break;
                }
            }

            yield return WaitComplete();
            
            setActionCompleted.Invoke();
            _onCompleted?.Invoke();
            
            // 播放特效完畢後，刪除所有特效
            yield return new WaitUntil(()=> !HaveFXPlaying());
            DestroyAllFxPlayers();
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

        private void DestroyAllFxPlayers()
        {
            foreach (var playingFX in playingFXs)
            {
                Debug.Log($"Destroy {playingFX.name}");
                GameObject.Destroy(playingFX.gameObject);
            }
        }

        public override string ToString()
        {
            return $"{nameof(_fxInfo)}: {_fxInfo}, {nameof(_effects)}: {_effects}, {nameof(TargetList)}: {TargetList}, {nameof(playingFXs)}: {playingFXs}";
        }
    }
}