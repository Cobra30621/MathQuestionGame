using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Action.Sequence
{
    public class SequenceManager : SerializedMonoBehaviour
    {
        [ShowInInspector]
        private Queue<ISequence> sequenceQueue = new Queue<ISequence>();

        [ShowInInspector] private ISequence currentSequence;
        
        [ShowInInspector]
        private bool isExecuting = false;

        public void AddSequence(ISequence action)
        {
            sequenceQueue.Enqueue(action);
            if (!isExecuting)
            {
                StartCoroutine(ExecuteActions());
            }
        }

        private IEnumerator ExecuteActions()
        {
            isExecuting = true;

            while (sequenceQueue.Count > 0)
            {
                currentSequence = sequenceQueue.Dequeue();
                bool actionCompleted = false;
                
                yield return currentSequence.Execute(() => actionCompleted = true);
                
                yield return new WaitUntil(() => actionCompleted);
            }

            isExecuting = false;
        }
    }
}