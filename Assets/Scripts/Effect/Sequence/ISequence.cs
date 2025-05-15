using System.Collections;

namespace Effect.Sequence
{
    /// <summary>
    /// 效果序列的抽象類別，所有特效流程都應繼承此基礎類別。
    /// </summary>
    public abstract class ISequence
    {
        /// <summary>
        /// 執行特效序列，結束時呼叫 setActionCompleted。
        /// </summary>
        public abstract IEnumerator Execute(System.Action setActionCompleted);
    }
}