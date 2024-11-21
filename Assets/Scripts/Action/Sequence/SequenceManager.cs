using System;
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

        [ShowInInspector]
        private bool actionCompleted;

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
                actionCompleted = false;
                try
                {
                    StartCoroutine(currentSequence.Execute(() =>
                    {
                        actionCompleted = true;
                    }));
                }
                catch (Exception e)
                {
                    Debug.LogError("Action Execution Failed:\n" +  e);
                    actionCompleted = true; // Assuming action completed in case of an error
                }
                
                yield return new WaitUntil(() => actionCompleted);
            }

            isExecuting = false;
        }
    }
}