using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Sequence
{
    public class DelaySequence : ISequence
    {
        [ShowInInspector]
        private List<EffectBase> _effects;
        
        [ShowInInspector]
        private float _delay = 0;


        public DelaySequence(List<EffectBase> effects,  float delay)
        {
            _effects = effects;
            _delay = delay;
        }

        public override IEnumerator Execute(Action setActionCompleted)
        {
            try
            {
                foreach (var effect in _effects)
                {
                    effect.Play();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Effect Execution Failed:\n" +  e);
            }
            

            yield return new WaitForSeconds(_delay);
            
            setActionCompleted.Invoke();
        }
    }
}