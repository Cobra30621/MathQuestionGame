using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Sequence
{
    /// <summary>
    /// 控制特效序列的執行管理器。
    /// 負責將加入的 ISequence 依照順序逐一執行，並確保每一個序列完成後才執行下一個。
    /// </summary>
    public class SequenceManager : SerializedMonoBehaviour
    {
        /// <summary>
        /// 儲存尚未執行的效果序列
        /// </summary>
        [ShowInInspector]
        private readonly Queue<ISequence> _pendingSequences = new();

        /// <summary>
        /// 當前正在執行的序列
        /// </summary>
        [ShowInInspector]
        private ISequence _currentSequence;

        /// <summary>
        /// 是否有序列正在執行中
        /// </summary>
        public bool IsExecuting => _isExecuting;

        [ShowInInspector]
        private bool _isExecuting = false;

        /// <summary>
        /// 當前序列是否執行完成
        /// </summary>
        [ShowInInspector]
        private bool _currentSequenceCompleted = false;

        /// <summary>
        /// 將新的序列加入隊列並觸發執行
        /// </summary>
        public void AddSequence(ISequence newSequence)
        {
            _pendingSequences.Enqueue(newSequence);
            
            // 若尚未執行任何序列則開始執行流程
            if (!_isExecuting)
            {
                StartCoroutine(ProcessSequenceQueue());
            }
        }

        /// <summary>
        /// 依序執行序列，確保每一個都完成後再繼續
        /// </summary>
        private IEnumerator ProcessSequenceQueue()
        {
            _isExecuting = true;

            while (_pendingSequences.Count > 0)
            {
                _currentSequence = _pendingSequences.Dequeue();
                _currentSequenceCompleted = false;

                // 執行當前序列，並傳入完成回呼
                yield return _currentSequence.Execute(() =>
                {
                    _currentSequenceCompleted = true;
                });

                // 確保 Execute 完成且完成旗標被設為 true
                yield return new WaitUntil(() => _currentSequenceCompleted);
            }

            _isExecuting = false;
        }
    }
}
