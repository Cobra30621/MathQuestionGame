using System.Collections;

namespace Effect.Sequence
{
    public abstract class ISequence
    {
        public abstract IEnumerator Execute(System.Action onComplete);
    }
}