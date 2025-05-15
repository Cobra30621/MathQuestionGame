using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Sequence
{
    /// <summary>
    /// 延遲執行序列：播放效果後等待固定秒數再完成。
    /// </summary>
    public class DelaySequence : ISequence
    {
        [ShowInInspector]
        private List<EffectBase> effectList;

        [ShowInInspector]
        private float delaySeconds = 0;

        public DelaySequence(List<EffectBase> effects, float delay)
        {
            this.effectList = effects;
            this.delaySeconds = delay;
        }

        /// <summary>
        /// 播放所有效果並等待指定秒數
        /// </summary>
        public override IEnumerator Execute(Action setActionCompleted)
        {
            try
            {
                foreach (var effect in effectList)
                {
                    effect.Play();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("DelaySequence Effect Execution Failed:\n" + e);
            }

            yield return new WaitForSeconds(delaySeconds);
            setActionCompleted.Invoke();
        }
    }
}