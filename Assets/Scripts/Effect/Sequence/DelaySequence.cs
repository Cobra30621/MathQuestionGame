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
        private List<EffectBase> _actions;
        
        [ShowInInspector]
        private float _delay = 0;


        public DelaySequence(List<EffectBase> actions,  float delay)
        {
            _actions = actions;
            _delay = delay;
        }

        public override IEnumerator Execute(System.Action onComplete)
        {
            try
            {
                foreach (var action in _actions)
                {
                    action.Play();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Action Execution Failed:\n" +  e);
            }
            

            yield return new WaitForSeconds(_delay);
            
            onComplete.Invoke();
        }
    }
}