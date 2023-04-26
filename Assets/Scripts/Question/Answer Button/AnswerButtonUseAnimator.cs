using UnityEngine;

namespace Question
{
    public class AnswerButtonUseAnimator : AnswerButtonBase
    {
        private Animator _animator;

        protected override void Init()
        {
            base.Init();
            _animator = GetComponent<Animator>();
        }

        public override void PlayOnAnswerAnimation(bool isCorrect)
        {
            // TODO animation 製作
            if (isCorrect)
            {
                
                // _animator.SetTrigger("Correct");
            }
            else
            {
                // _animator.SetTrigger("Wrong");
            }
        }
    }
}