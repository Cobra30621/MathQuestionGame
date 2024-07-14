using System.Collections;
using System.Collections.Generic;
using NueGames.Action;
using UnityEngine;

namespace Action.Sequence
{
    public class DelaySequence : ISequence
    {
        private List<GameActionBase> _actions;
        private float _delay = 0;


        public DelaySequence(List<GameActionBase> actions,  float delay)
        {
            _actions = actions;
            _delay = delay;
        }

        public override IEnumerator Execute(System.Action onComplete)
        {
            foreach (var action in _actions)
            {
                action.DoAction();
            }

            yield return new WaitForSeconds(_delay);
            
            onComplete.Invoke();
        }
    }
}