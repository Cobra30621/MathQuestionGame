using System.Collections;
using System.Collections.Generic;
using Effect.Sequence;
using Managers;
using Sirenix.OdinInspector;

namespace Effect
{
    /// <summary>
    /// 負責執行遊戲行為（Effect）
    /// 提供 AddToTop, AddAction 方法，讓其他人加入想要執行的（Effect）
    /// </summary>
    public class EffectExecutor : SerializedMonoBehaviour
    {

        /// <summary>
        /// 單例模式
        /// </summary>
        public static EffectExecutor Instance => GameManager.Instance.effectManager;

        public SequenceManager SequenceManager;

        public bool IsExecuting => SequenceManager.IsExecuting;
        
        
        #region 將遊戲行為加入執行緒

        public static void ExecuteImmediately(EffectBase action)
        {
            action.DoAction();
        }
        
        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲效果清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(EffectBase action, float delay = 0.1f)
        {
            Instance.SequenceManager.AddSequence(new DelaySequence(new List<EffectBase>(){action}, delay));
        }
        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲效果清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(List<EffectBase> actions, float delay = 0.1f)
        {
            Instance.SequenceManager.AddSequence(new DelaySequence(actions, delay));
        }

        public static void AddActionWithFX(FXSequence fxSequence)
        {
            Instance.SequenceManager.AddSequence(fxSequence);
        }

        public static void DoCoroutine(IEnumerator coroutine)
        {
            Instance.StartCoroutine(coroutine);
        }
        

        #endregion

    }

}