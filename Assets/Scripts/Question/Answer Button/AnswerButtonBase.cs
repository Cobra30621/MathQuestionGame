using UnityEngine;
using UnityEngine.UI;

namespace Question
{
    public abstract  class AnswerButtonBase : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            button = GetComponent<Button>();
        }


        public abstract void PlayOnAnswerAnimation(bool isCorrect);

        public void EnableAnswer(bool enable)
        {
            button.enabled = enable;
        }
    }
}