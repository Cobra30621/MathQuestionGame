using System.Collections;

namespace Action.Sequence
{
    public abstract class ISequence
    {
        public abstract IEnumerator Execute(System.Action onComplete);
    }
}