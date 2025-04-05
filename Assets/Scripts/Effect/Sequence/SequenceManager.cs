using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Sequence
{
    /// <summary>
    /// 遊戲效果的執行序
    /// </summary>
    public class SequenceManager : SerializedMonoBehaviour
    {
        [ShowInInspector]
        private Queue<ISequence> sequenceQueue = new Queue<ISequence>();

        [ShowInInspector] private ISequence currentSequence;

        public bool IsExecuting => isExecuting;
        
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

                yield return currentSequence.Execute(() =>
                {
                    actionCompleted = true;
                });
                
                yield return new WaitUntil(() => actionCompleted);
            }

            isExecuting = false;
        }
    }
}