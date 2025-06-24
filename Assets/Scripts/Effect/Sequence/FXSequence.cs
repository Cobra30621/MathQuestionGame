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
    /// <summary>
    /// 表示一段特效序列，包含多個 EffectBase 與實體 FX 播放。
    /// </summary>
    [Serializable]
    public class FXSequence : ISequence
    {
        [ShowInInspector]
        private FxInfo fxInfo;

        [ShowInInspector]
        private List<EffectBase> effectList;

        [ShowInInspector]
        protected List<CharacterBase> targetCharacters;

        protected FxManager FxManager => FxManager.Instance;

        [ShowInInspector]
        private List<FXPlayer> activeFXPlayers;

        private Action onSequenceComplete;


        public FXSequence(List<EffectBase> effects, FxInfo fxInfo, List<CharacterBase> targets, Action onComplete)
        {
            this.fxInfo = fxInfo;
            this.effectList = effects;
            this.targetCharacters = targets;
            this.onSequenceComplete = onComplete;
        }

        /// <summary>
        /// 執行特效播放與邏輯效果，同步或延遲等候。
        /// </summary>
        public override IEnumerator Execute(Action setActionCompleted)
        {
            // 播放邏輯效果
            foreach (var effect in effectList)
            {
                effect.Play();
            }

            activeFXPlayers = new List<FXPlayer>();
            FXPlayer fxPlayer;

            // 播放特效
            if (fxInfo.FxPrefab != null)
            {
                var spawnTransform = FxManager.GetFXSpawnPosition(fxInfo.FxSpawnPosition);
                switch (fxInfo.FxSpawnPosition)
                {
                    case FxSpawnPosition.EachTarget:
                        foreach (var target in targetCharacters)
                        {
                            fxPlayer = FxManager.PlayFx(fxInfo.FxPrefab, spawnTransform, target.transform.position);
                            activeFXPlayers.Add(fxPlayer);
                            fxPlayer.Play();
                        }
                        break;
                    case FxSpawnPosition.Ally:
                        spawnTransform.position = CombatManager.Instance.GetMainAllyTransform().position;
                        fxPlayer = FxManager.PlayFx(fxInfo.FxPrefab, spawnTransform);
                        fxPlayer.Play();
                        activeFXPlayers.Add(fxPlayer);
                        break;
                    case FxSpawnPosition.EnemyMiddle:
                    case FxSpawnPosition.ScreenMiddle:
                        fxPlayer = FxManager.PlayFx(fxInfo.FxPrefab, spawnTransform);
                        fxPlayer.Play();
                        activeFXPlayers.Add(fxPlayer);
                        break;
                }
            }

            yield return WaitUntilReady();

            setActionCompleted.Invoke();
            onSequenceComplete?.Invoke();

            
            EffectExecutor.Instance.StartCoroutine(DestroyAllFXWhenCompleted());
        }

        /// <summary>
        /// 根據 WaitMethod 等待特效完成或延遲時間。
        /// </summary>
        private IEnumerator WaitUntilReady()
        {
            if (fxInfo.WaitMethod == WaitMethod.WaitFXFinish)
            {
                yield return new WaitUntil(() => !IsAnyFXPlaying());
            }
            else if (fxInfo.WaitMethod == WaitMethod.WaitDelay)
            {
                yield return new WaitForSeconds(fxInfo.Delay);
            }
        }

        /// <summary>
        /// 檢查是否還有特效正在播放。
        /// </summary>
        private bool IsAnyFXPlaying()
        {
            foreach (var fx in activeFXPlayers)
            {
                if (fx.IsPlaying()) return true;
            }
            return false;
        }

        /// <summary>
        /// 銷毀所有播放過的特效。
        /// </summary>
        private IEnumerator DestroyAllFXWhenCompleted()
        {
            // 等待所有特效結束後銷毀
            yield return new WaitUntil(() => !IsAnyFXPlaying());
            
            foreach (var fx in activeFXPlayers)
            {
                Debug.Log($"Destroy FX: {fx.name}");
                GameObject.Destroy(fx.gameObject);
            }
        }

        public override string ToString()
        {
            return $"FX Info: {fxInfo}, Effects: {effectList}, Targets: {targetCharacters}, FX Instances: {activeFXPlayers}";
        }
    }
}
