using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.Action;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Action.Sequence
{
    public class DelaySequence : ISequence
    {
        [ShowInInspector]
        private List<GameActionBase> _actions;
        
        [ShowInInspector]
        private float _delay = 0;


        public DelaySequence(List<GameActionBase> actions,  float delay)
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
                    action.DoAction();
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